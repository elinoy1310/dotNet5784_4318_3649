using BlApi;
using BO;
using DalApi;
using DO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace BlImplementation;

internal class EngineerImplementation : BlApi.IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();
    private readonly IBl bl;
    internal EngineerImplementation(IBl bl) => this.bl = bl;

    /// <summary>
    /// Creates a new engineer in the data layer.
    /// </summary>
    /// <param name="engineer">The engineer object containing the details of the new engineer.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    /// <exception cref="BlWrongDataException">Thrown when the provided data for the engineer is invalid.</exception>
    /// <exception cref="BlAlreadyExistException">Thrown when an engineer with the same ID already exists.</exception>
    public int Create(BO.Engineer engineer)
    {
        // Check project status and engineer's task before creating
        if (_bl.CheckProjectStatus() != ProjectStatus.Execution && engineer.Task is not null)
            throw new BlWrongDataException("Can't add engineer with task before the project had started");
        // Validate engineer data
        if (engineer.Id <= 0 || engineer.Name == null || engineer.Cost <= 0 || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty))
            throw new BlWrongDataException("Invalid data");
        // Create a new data layer engineer object
        DO.Engineer newEngineer = new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost, engineer.Name, (DO.EngineerExperience)engineer.level);
        try
        {
            // Attempt to create the engineer in the data layer
            int idNewEnginerr = _dal.Engineer.Create(newEngineer);

            return idNewEnginerr;
        }

        catch (DO.DalAlreadyExistException ex)
        {
            // Handle the case where an engineer with the same ID already exists
            throw new BlAlreadyExistException($"Engineer with ID={engineer.Id} already exists", ex);
        }
    }

    /// <summary>
    /// Deletes an engineer from the data layer.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    /// <exception cref="BlCannotBeDeletedException">Thrown when the engineer cannot be deleted due to active or completed tasks.</exception>
    /// <exception cref="BlDoesNotExistException">Thrown when the specified engineer does not exist.</exception>
    public void Delete(int id)
    {
        // Check if the engineer has active or completed tasks
        if (createTaskInEngineer(id) is not null)
            throw new BlCannotBeDeletedException("Can't Delete an Engineer who has already finished performing a task or is actively performing a task");
        try
        {
            // Attempt to delete the engineer from the data layer
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            // Handle the case where the engineer to delete does not exist
            throw new BlDoesNotExistException($"Engineer with ID={id} was not found", ex);
        }

    }

    /// <summary>
    /// Retrieves the details of an engineer from the data layer.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer object with the specified ID.</returns>
    /// <exception cref="BlDoesNotExistException">Thrown when the specified engineer does not exist.</exception>
    public BO.Engineer Read(int id)
    {
        // Retrieve the engineer details from the data layer
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        // Check if the engineer exists
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        // Create and return the business object representation of the engineer
        return new BO.Engineer
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            level = (BO.EngineerExperience)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = createTaskInEngineer(doEngineer.Id)
        };

    }

    /// <summary>
    /// Retrieves the details of an engineer from the data layer based on the specified filter.
    /// </summary>
    /// <param name="filter">An optional filter to apply when retrieving the engineer.</param>
    /// <returns>The engineer object matching the filter criteria.</returns>
    /// <exception cref="BlDoesNotExistException">Thrown when no engineer matches the specified filter.</exception>
    public BO.Engineer Read(Func<BO.Engineer, bool>? filter = null)
    {
        // Check if a filter is provided
        if (filter == null)
            // If no filter is provided, return the first engineer, or throw an exception if there are no engineers
            return ReadAll().FirstOrDefault() ?? throw new BlDoesNotExistException("there are 0 engineers right now");
        else
            // Apply the filter and return the first engineer matching the filter, or throw an exception if no match is found
            return ReadAll().FirstOrDefault(filter) ?? throw new BlDoesNotExistException($"Engineer with {filter} does not exist");
    }

    /// <summary>
    /// Retrieves all engineers from the data layer based on the specified filter.
    /// </summary>
    /// <param name="filter">An optional filter to apply when retrieving engineers.</param>
    /// <returns>A collection of engineer objects matching the filter criteria.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        // Retrieve all engineers from the data layer
        var engineers = from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        select Read(doEngineer.Id);
        // Check if a filter is provided
        if (filter == null)
            // If no filter is provided, return all engineers
            return engineers;
        else
        // Apply the filter and return engineers matching the filter criteria
        return engineers.Where(filter);
    }

    /// <summary>
    /// Updates the details of the previous task with the specified ID.
    /// </summary>
    /// <param name="idTask">The ID of the task to update.</param>
    private void updatePreviousTask(int idTask)
    {
        // Retrieve the previous task from the data layer and update its details
        _dal.Task.Update(_dal.Task.Read(idTask)! with { CompleteDate = bl.Clock, EngineerId = null });
    }

    /// <summary>
    /// Updates the details of a new task with the specified ID, engineer ID, and engineer level.
    /// </summary>
    /// <param name="idTask">The ID of the task to update.</param>
    /// <param name="idEngineer">The ID of the engineer assigned to the task.</param>
    /// <param name="engineerLevel">The experience level of the engineer.</param>
    /// <exception cref="BlCannotBeUpdatedException">
    /// Thrown when the task cannot be started, the task's level is beyond the engineer's level,
    /// or another engineer is already assigned to the task.
    /// </exception>
    private void updateNewTask(int idTask, int idEngineer, BO.EngineerExperience engineerLevel)
    {
        // Retrieve the task from the data layer
        DO.Task t = _dal.Task.Read(idTask)!;
        // Check if the task can be started
        if (t.ScheduledDate > bl.Clock)
            throw new BlCannotBeUpdatedException("the task can't be started");
        // Check if the task's level is beyond the engineer's level
        if ((int)t.Complexity > (int)engineerLevel)
            throw new BlCannotBeUpdatedException("the task's level is beyond the engineer's level");
        // Check if the task already has an assigned engineer
        if (t.EngineerId == null)
            // Update the task with the new engineer and start date
            _dal.Task.Update(t with { EngineerId = idEngineer, StartDate = bl.Clock });
        else
            throw new BlCannotBeUpdatedException("other engineer take care of the new engineer's task");
    }

    /// <summary>
    /// Updates the details of the specified engineer.
    /// </summary>
    /// <param name="engineer">The engineer object containing the updated details.</param>
    /// <exception cref="BlCannotBeUpdatedException">Thrown when attempting to update an engineer with a task before the project has started.</exception>
    /// <exception cref="BlWrongDataException">Thrown when the provided data for the engineer is invalid.</exception>
    /// <exception cref="BlDoesNotExistException">Thrown when the specified engineer does not exist.</exception>
    public void Update(BO.Engineer engineer)
    {
        // Check if the project has started and the engineer has a task assigned
        if (_bl.CheckProjectStatus() != ProjectStatus.Execution && engineer.Task is not null)
            throw new BlCannotBeUpdatedException("Can't update engineer with task before the project had started");
        // Retrieve the engineer to update
        BO.Engineer toUpdateEngineer = Read(engineer.Id);
        // Validate the updated engineer data
        if (engineer.Name == null || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty) || engineer.level < toUpdateEngineer.level || engineer.Cost <= 0)
            throw new BlWrongDataException("Invalid data");
        // Check if the task assignment of the engineer has changed
        if (engineer.Task != toUpdateEngineer.Task)
        {
            if (engineer.Task == null)
                updatePreviousTask((int)toUpdateEngineer.Task!.Id!);
            else if (toUpdateEngineer.Task == null)
                updateNewTask((int)engineer.Task!.Id!, engineer.Id, engineer.level);
            else
            {
                updateNewTask((int)engineer.Task!.Id!, (int)engineer.Id, engineer.level);
                updatePreviousTask((int)toUpdateEngineer.Task!.Id!);
            }
        }

        try
        {
            // Create a new data layer engineer object with the updated details and update in the data layer
            DO.Engineer updatedEngineer = new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost, engineer.Name, (DO.EngineerExperience)engineer.level);
            _dal.Engineer.Update(updatedEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            // Handle the case where the engineer is not found in the data layer
            throw new BlDoesNotExistException($"Engineer with ID={engineer.Id} was not found", ex);
        }
    }

    /// <summary>
    /// Creates a TaskInEngineer object for the engineer with the specified ID, if any task is assigned to the engineer.
    /// </summary>
    /// <param name="id">The ID of the engineer.</param>
    /// <returns>A TaskInEngineer object representing the task assigned to the engineer, or null if no task is assigned.</returns>
    private TaskInEngineer? createTaskInEngineer(int id)
    {
        // Find the task assigned to the engineer
        var task = _dal.Task.ReadAll().FirstOrDefault(item => item is not null && item.EngineerId == id);
        // If a task is found, create and return a TaskInEngineer object
        if (task is not null)
            return new TaskInEngineer { Id = task.Id, Alias = task.Alias };
        // Return null if no task is assigned to the engineer
        return null;
    }


    /// <summary>
    /// Retrieves engineers from a certain experience level or higher.
    /// </summary>
    /// <param name="level">The minimum experience level of the engineers to retrieve.</param>
    /// <returns>A collection of all engineers with experience level greater than or equal to the specified level.</returns>
    public IEnumerable<BO.Engineer> EngineersFromLevel(BO.EngineerExperience level)
    {
        // Group engineers by their experience level
        var engGroup = from BO.Engineer boEngineer in ReadAll()
                       group boEngineer by boEngineer.level into g
                       //where g.Key >= level
                       select g;
        List<BO.Engineer> engineers=new List<BO.Engineer>();
        // Iterate through each group and add engineers with experience level greater than or equal to the specified level
        foreach (var gro in engGroup)
        {
            if (gro.Key >= level)
                foreach (var g in gro)
                    engineers.Add(g);
        }
        return engineers;

    }

    /// <summary>
    /// Retrieves a collection of engineers sorted by name, experience level, and cost.
    /// </summary>
    /// <returns>A collection of engineers sorted by name, experience level, and cost.</returns>
    public IEnumerable<BO.Engineer> sortedByName()
    {
        // Retrieve engineers from the data layer and sort them by name, experience level, and cost
        return from BO.Engineer boEngineer in ReadAll()
               orderby boEngineer.Name,boEngineer.level,boEngineer.Cost
               select boEngineer;
    }
}
