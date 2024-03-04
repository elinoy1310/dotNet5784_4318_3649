using BlApi;
using BO.Engineer;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();

    /// <summary>
    /// Creates a new task based on the provided business object and adds it to the system.
    /// </summary>
    /// <param name="task">The business object representing the task to be created.</param>
    /// <returns>The ID of the newly created task.</returns>
    public int Create(BO.Engineer.Task task)
    {
        // Check if project status allows task creation
        if (_bl.CheckProjectStatus() == BO.Engineer.ProjectStatus.Execution)
            throw new BlWrongDataException("Cannot add a task when the project has started");
        // Validate task data
        if (task.Id >= 0 && task.Alias != null)
        {
            int? engineerId = task.Engineer is not null ? task.Engineer.Id : null;
            // Create data object from business object
            DO.Task doTask = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, engineerId,(DO.EngineerExperience)task.Complexity);
            // Create task in the data layer and get the new task ID
            int idNewTask = _dal.Task.Create(doTask);
            // Create dependencies for the new task
            if (task.Dependencies is not null)
            {
                foreach (var d in task.Dependencies)
                {
                    _dal.Dependency.Create(new DO.Dependency(0,idNewTask, d.Id));
                }
            }
            return idNewTask;
        }
        else
            throw new BlWrongDataException("Invalid data");

    }

    /// <summary>
    /// Sets the start date for a task identified by the specified ID, after performing necessary validations.
    /// </summary>
    /// <param name="id">The ID of the task for which to set the start date.</param>
    /// <param name="date">The start date to be set for the task.</param>
    /// <exception cref="BlNotUpdatedDataException">
    /// Thrown when not all start dates of previous tasks of the specified task have been updated.
    /// </exception>
    /// <exception cref="BlWrongDataException">
    /// Thrown when the specified date is earlier than all the estimated end dates of the previous tasks for the task with the specified ID.
    /// </exception>
    public void CreateStartDate(int id, DateTime date)
    {
        // Retrieve dependencies of the task
        List<TaskInList> list = returnDepTask(id).ToList();
        // Check if all start dates of previous tasks are updated
        var tempListStatus = list.Select(task => task.Status == Status.Unscheduled).ToList();
        if (tempListStatus != null)
            throw new BlNotUpdatedDataException($"Not all start dates of previous tasks of task with ID={id} updates");
        // Find tasks with end dates later than the specified start date
        var findTask = from taskInAllTasks in _dal.Task.ReadAll()
                       from depTask in list
                       where taskInAllTasks.Id == depTask.Id
                       select taskInAllTasks;
        var tempListDate = from task in findTask
                           where date < task.CompleteDate
                           select task;
        // Check if the specified date is earlier than all the estimated end dates of the previous tasks
        if (tempListDate!=null)
            throw new BlWrongDataException($"The date is earlier than all the estimated end dates of the previous tasks for the task with ID={id}");
        // Read the task from the data layer
        DO.Task updateTask = _dal.Task.Read(id) ?? throw new NotImplementedException();
        // Update the task's ScheduledDate property with the specified date
        _dal.Task.Update(updateTask with {ScheduledDate=date});
    }

    /// <summary>
    /// Deletes the task with the specified ID after performing necessary validations.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <exception cref="BlCannotBeDeletedException">
    /// Thrown when the task cannot be deleted due to project execution status or existing dependencies.
    /// </exception>
    /// <exception cref="BlDoesNotExistException">
    /// Thrown when the task with the specified ID does not exist in the data layer.
    /// </exception>
    public void Delete(int id)
    {
        // Check if project status allows task deletion
        if (_bl.CheckProjectStatus()==BO.Engineer.ProjectStatus.Execution)
            throw new BlCannotBeDeletedException($"Task with ID={id} cannot be deleted after the project started");
        // Read the task with the specified ID
        BO.Engineer.Task DelTask = Read(id);
        // Check if the task has any dependencies
        if ( _dal.Dependency.ReadAll().FirstOrDefault(d => d?.DependsOnTask == id) != null) 
            throw new BlCannotBeDeletedException($"Task with ID={id} cannot be deleted");
        try
        {
            // Attempt to delete the task from the data layer
            _dal.Task.Delete(DelTask.Id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            // Throw exception if the task does not exist in the data layer
            throw new BlDoesNotExistException($"Task with ID={id} was not found", ex);
        }
    }

    /// <summary>
    /// Reads and retrieves the task with the specified ID from the data layer.
    /// </summary>
    /// <param name="id">The ID of the task to read.</param>
    /// <returns>The task object with the specified ID.</returns>
    /// <exception cref="BlDoesNotExistException">
    /// Thrown when the task with the specified ID does not exist in the data layer.
    /// </exception>
    public BO.Engineer.Task Read(int id)
    {
        // Read the task from the data layer
        DO.Task? taskRead = _dal.Task.Read(id);
        // Check if the task exists
        if (taskRead == null)
            throw new BlDoesNotExistException($"Task with ID={id} does Not exist");
        // Convert the task of the data layer to the task of the logical layer
        return converFromDOtoBO(taskRead);
    }

    /// <summary>
    /// Reads and retrieves a task from the data layer based on the specified filter.
    /// </summary>
    /// <param name="filter">The filter condition to apply when retrieving the task.</param>
    /// <returns>The task object that satisfies the filter condition, or <c>null</c> if no such task is found.</returns>
    public BO.Engineer.Task? Read(Func<BO.Engineer.Task, bool> filter)
    {
        // Convert all data tasks to logical tasks
        IEnumerable<BO.Engineer.Task> convertAll= from item in _dal.Task.ReadAll()
                                          let convertItem = converFromDOtoBO(item)
                                          select convertItem;
        // Apply the filter to the collection of tasks
        if (filter != null)
            return convertAll.FirstOrDefault(filter);
        // Return the first task if no filter is provided
        return convertAll.FirstOrDefault();
    }

    /// <summary>
    /// Retrieves a collection of all tasks from the data layer, optionally filtered by a provided condition.
    /// </summary>
    /// <param name="filter">The filter condition to apply when retrieving the tasks. If <c>null</c>, no filtering is applied.</param>
    /// <returns>
    /// An enumerable collection of task objects that satisfy the filter condition, or all tasks if no filter is provided.
    /// </returns>
    public IEnumerable<BO.Engineer.TaskInList> ReadAll(Func<BO.Engineer.Task, bool>? filter = null) //לא מדפיס הכל ולא לזרוק שגיאה אם לא מוצא את המהנדס
    {
        if (filter!=null)
        {
            // Apply the filter condition to the collection of tasks
            IEnumerable<BO.Engineer.Task> isFilter = from item in _dal.Task.ReadAll()
                                            let convertItem = converFromDOtoBO(item)
                                            where filter(convertItem)
                                            select convertItem;
            // Convert filtered tasks to TaskInList objects
            return (from item in isFilter 
                    select new BO.Engineer.TaskInList { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = item.Status });
        }
        else
            // Retrieve all tasks from the data layer and convert them to TaskInList objects
            return (from item in _dal.Task.ReadAll() 
                    let convertItem = converFromDOtoBO(item)
                    select new BO.Engineer.TaskInList { Id = convertItem.Id, Alias = convertItem.Alias, Description = convertItem.Description, Status = convertItem.Status });
    }

    /// <summary>
    /// Updates the details of a task in the data layer.
    /// </summary>
    /// <param name="task">The task object containing the updated details.</param>
    /// <exception cref="BlDoesNotExistException">Thrown when the specified task does not exist.</exception>
    public void Update(BO.Engineer.Task task)
    {
        // Retrieve the original task from the data layer
        BO.Engineer.Task Originaltask = Read(task.Id);
        // Check the project status to determine the update logic
        if (_bl.CheckProjectStatus() == BO.Engineer.ProjectStatus.Planing)
        {
            // Update dependencies and task details
            updateDependencies(task);
            try
            {
                // Convert the logic object task to a data object and update in the data layer
                DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, Originaltask.CreatedAtDate, Originaltask.ScheduledDate, Originaltask.StartDate, Originaltask.CompleteDate, null, task.Deliverables, task.Remarks, Originaltask.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
                _dal.Task.Update(convertFromBOtoDO);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                // Handle the case where the task is not found in the data layer
                throw new BlDoesNotExistException($"Task with ID={task.Id} was not found", ex);
            }
        }
        else if (_bl.CheckProjectStatus() == BO.Engineer.ProjectStatus.Mid)
        {
            // Perform additional checks and update logic based on project status
            CheckingEngineer(task);
            updateDependencies(task);
            try
            {
                // Convert the logic object task to a data object and update in the data layer
                DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, task.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
                _dal.Task.Update(convertFromBOtoDO);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                // Handle the case where the task is not found in the data layer
                throw new BlDoesNotExistException($"Task with ID={task.Id} was not found", ex);
            }
        }
        else if(_bl.CheckProjectStatus() == BO.Engineer.ProjectStatus.Execution)
        {
            CheckingEngineer(task);
            try
            {
                // Convert the logic object task to a data object and update in the data layer
                DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, task.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
                _dal.Task.Update(convertFromBOtoDO);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                // Handle the case where the task is not found in the data layer
                throw new BlDoesNotExistException($"Task with ID={task.Id} was not found", ex);
            }
        }
        
    }

    /// <summary>
    /// Checks if the assigned engineer for the task exists and has the required level of expertise.
    /// </summary>
    /// <param name="task">The task object containing the engineer details.</param>
    /// <exception cref="BlDoesNotExistException">Thrown when the assigned engineer does not exist.</exception>
    /// <exception cref="BlWrongDataException">Thrown when the engineer's level is insufficient for the task.</exception>
    private void CheckingEngineer(BO.Engineer.Task task)
    {
        // Check if an engineer is assigned to the task
        if (task.Engineer != null)
        {
            // Retrieve the engineer from the data layer
            DO.Engineer? engInTask = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.Engineer?.Id);
            // Check if the engineer exists
            if (engInTask == null)
                throw new BlDoesNotExistException($"Engineer with ID={task.Engineer?.Id} was not found");
            // Compare the engineer's level with the task's complexity level
            if ((int)engInTask.Level < (int)task.Complexity)
                throw new BlWrongDataException("The level of the engineer is too low for the level of the task");
        }
    }

    /// <summary>
    /// Updates the dependencies of the specified task in the data layer.
    /// </summary>
    /// <param name="task">The task object containing the updated dependencies.</param>
    private void updateDependencies(BO.Engineer.Task task)
    {
        // Check if the task has dependencies
        if (task.Dependencies is not null)
        {
            // Iterate through the dependencies of the task
            foreach (var d in task.Dependencies)
            {
                bool isExist = false;
                // Check if the dependency already exists in the data layer
                foreach (var dep in _dal.Dependency.ReadAll())
                {
                    if (dep?.Dependent == task.Id && dep.DependsOnTask == d.Id)
                        isExist = true;
                }
                // If the dependency does not exist, create it in the data layer
                if (!isExist)
                    _dal.Dependency.Create(new DO.Dependency(0, task.Id, d.Id));
            }
        }
    }

    //private void deleteDependencies(BO.Task task)
    //{
    //    if (task.Dependencies is not null)
    //    {
    //        foreach (var d in task.Dependencies)
    //        {
    //            int dependencyId = 0;
    //            bool isExist = false;
    //            {
    //                    foreach (var dep in _dal.Dependency.ReadAll())
    //                    {
    //                        if (dep?.Dependent == task.Id && dep.DependsOnTask == d.Id)
    //                        {
    //                            dependencyId = dep.Id;
    //                            isExist = true;
    //                        }
    //                    if (isExist)
    //                    {
    //                        _dal.Dependency.Delete(dependencyId);
    //                    }
    //                    isExist = false;
    //                    }

    //            }
    //        }
    //        task.Dependencies = returnDepTask(task.Id);
    //    }
    //}

    /// <summary>
    /// Calculates the status of the specified task based on its scheduled, start, and completion dates.
    /// </summary>
    /// <param name="task">The task object for which to calculate the status.</param>
    /// <returns>The calculated status of the task.</returns>
    private Status statusCalculation(DO.Task task)
    {
        Status status = 0;
        // Check if the scheduled date is null
        if (task.ScheduledDate == null)
            status = Status.Unscheduled;
        // Check if the start date is null
        else if (task.StartDate == null)
            status = Status.Scheduled;
        // Check if the completion date is null
        else if (task.CompleteDate == null)
            status = Status.OnTrack;
        // If all dates are not null, set status to Done
        else
            status = Status.Done;
        return status;
    }

    /// <summary>
    /// Returns a list of tasks that depend on the task with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the task for which dependent tasks are to be retrieved.</param>
    /// <returns>An enumerable collection of dependent tasks.</returns>
    public IEnumerable<TaskInList> returnDepTask(int id)
    {
        // Create a list to store dependent tasks
        List<TaskInList> depTaskInLists = new List<TaskInList>();
        // Query dependent tasks from the data layer
        var depList = from dep in _dal.Dependency.ReadAll()
                      where (dep.Dependent == id)
                      select dep.DependsOnTask;
        // Iterate through dependent task IDs and retrieve corresponding tasks
        foreach (var item in depList)
        {
            // Retrieve dependent task from the data layer
            DO.Task depTask = _dal.Task.ReadAll().FirstOrDefault(t => t?.Id == item) ?? throw new BlDoesNotExistException($"Task with ID={item} does Not exist");
            // Add dependent task to the list with calculated status
            depTaskInLists.Add(new TaskInList { Id = depTask.Id, Alias = depTask.Alias, Description = depTask.Description, Status = statusCalculation(depTask) });
        }
        return depTaskInLists;
    }

    /// <summary>
    /// Returns an EngineerInTask object representing the engineer assigned to the specified task.
    /// </summary>
    /// <param name="task">The task for which the assigned engineer is to be retrieved.</param>
    /// <returns>An EngineerInTask object representing the assigned engineer, or null if no engineer is assigned.</returns>
    private EngineerInTask returnEngineerOnTask(DO.Task task)
    {
        // Check if an engineer is assigned to the task
        if (task.EngineerId == null)
            return null!;
        else
        {
            // Retrieve the engineer from the data layer
            DO.Engineer eng = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.EngineerId) ?? throw new BlDoesNotExistException($"Engineer with ID={task.EngineerId} does Not exist");
            // Return an EngineerInTask object representing the assigned engineer
            return new EngineerInTask { Id = eng.Id, Name = eng.Name };
        }
    }

    /// <summary>
    /// Calculates and returns the forecasted completion date based on scheduled date, start date, and required effort time.
    /// </summary>
    /// <param name="scheduledDate">The scheduled start date of the task.</param>
    /// <param name="startDate">The actual start date of the task.</param>
    /// <param name="requiredEffortTime">The amount of time required to complete the task.</param>
    /// <returns>The forecasted completion date of the task.</returns>
    private DateTime? returnForecastDate (DateTime? scheduledDate, DateTime? StartDate, TimeSpan? RequiredEffortTime)
    {
        // If the task has not started yet, forecast based on scheduled date
        if (StartDate==null)
            return (scheduledDate + RequiredEffortTime);
        // Otherwise, forecast based on the later of scheduled date and actual start date
        return ((scheduledDate > StartDate) ? scheduledDate : StartDate) + RequiredEffortTime;
    }

    /// <summary>
    /// Converts a data layer task object to a business object task object.
    /// </summary>
    /// <param name="task">The data layer task object to be converted.</param>
    /// <returns>The converted business object task object.</returns>
    private BO.Engineer.Task converFromDOtoBO(DO.Task task)
    {
        // Create a new business object task instance and populate its properties from the data layer task object
        BO.Engineer.Task newTask=
         new BO.Engineer.Task()
        {
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            CreatedAtDate = task.CreatedInDate,
            Status = statusCalculation(task),
            Dependencies = returnDepTask(task.Id),
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = task.ScheduledDate,
            ForecastDate = returnForecastDate(task.ScheduledDate, task.StartDate, task.RequiredEffortTime),
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = returnEngineerOnTask(task),
            Complexity = (BO.Engineer.EngineerExperience)task.Complexity
        };
        return newTask;
    }
}
