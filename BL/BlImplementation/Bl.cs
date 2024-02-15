

using BlApi;
using BO;
using DalApi;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

public class Bl : IBl
{
    public BlApi.IEngineer Engineer => new EngineerImplementation();

    public BlApi.ITask Task => new TaskImplementation();

    private DalApi.IDal _dal = DalApi.Factory.Get;
    public DateTime? ProjectStartDate { get => _dal.ProjectStartDate; set => _dal.ProjectStartDate=value; }

    /// <summary>
    /// Checks the status of the project based on its tasks and scheduled start date.
    /// </summary>
    /// <returns>The status of the project.</returns>
    public ProjectStatus CheckProjectStatus()
    {
        // If the project start date is not set, it's in the planning phase
        if (ProjectStartDate == null)
            return ProjectStatus.Planing;
        // If any task's scheduled date is not set, the project is in execution phase
        if (Task.ReadAll(item=>item.ScheduledDate is null)is null)         
            return ProjectStatus.Execution;
        // Otherwise, the project is in the mid phase
        return ProjectStatus.Mid;
    }

    /// <summary>
    /// Creates a schedule for tasks based on the specified option.
    /// </summary>
    /// <param name="option">The option for creating the schedule (Automatically or Manually).</param>
    /// <param name="taskId">The ID of the task for manual scheduling (optional, required if option is Manually).</param>
    /// <exception cref="BlWrongInputFormatException">Thrown when the input format is incorrect.</exception>
    /// <exception cref="BlDoesNotExistException">Thrown when a task does not exist.</exception>
    public void CreateSchedule(CreateScheduleOption option/*=CreateScheduleOption.Manually*/, int taskId)
    {
        switch (option)
        {
            case CreateScheduleOption.Automatically:
                createScheduleAuto();
                break;
            case CreateScheduleOption.Manually:
                createScheduleOptionManually(taskId);
                break;
        }

    }

    /// <summary>
    /// Creates a schedule for a task manually based on its dependencies.
    /// </summary>
    /// <param name="taskId">The ID of the task for manual scheduling.</param>
    /// <exception cref="BlDoesNotExistException">Thrown when a task or its dependencies do not exist.</exception>
    /// <exception cref="BlCanNotBeNullException">Thrown when a task's forecast date is not set for a dependency.</exception>
    private void createScheduleOptionManually(int taskId)
    {
        BO.Task task = Task.Read(taskId);
        if (task.Dependencies == null)
        {
            task.ScheduledDate = ProjectStartDate;
        }
            DateTime? maxForecast = DateTime.Now;
        foreach (var d in task.Dependencies!)
        {
            BO.Task readTask=Task.Read(d.Id);
            if (readTask.ForecastDate == null)
                throw new BlCanNotBeNullException("It is not possible to update a task to the previous task no forecast date has been set.");
            if (readTask.ForecastDate > maxForecast)
                maxForecast = readTask.ForecastDate;  
        }
        task.ScheduledDate = maxForecast;
        Task.Update(task);
    }

    /// <summary>
    /// Creates a schedule for tasks automatically based on their dependencies and project start date.
    /// </summary>
    /// <remarks>
    /// Assumptions:
    /// - The project start date is available.
    /// - Each task has a duration and complexity level.
    /// - Tasks may have dependencies.
    /// - This method updates the scheduled date for each task based on its dependencies and project start date.
    /// </remarks>
    /// <exception cref="BlDoesNotExistException">Thrown when a task or its dependencies do not exist.</exception>
    private void createScheduleAuto()
    {
        // Assumptions:
        // - The project start date is available.
        // - Each task has a duration and complexity level.
        // - Tasks may have dependencies.
        // - This method updates the scheduled date for each task based on its dependencies and project start date.

        DateTime start = _dal.ProjectStartDate!.Value;
        IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList();
        // Update scheduled dates for tasks without dependencies to project start date
        foreach (TaskInList task in tasksWithoutDep)
        {
            BO.Task taskWithStartDate = Task.Read(task.Id);
            taskWithStartDate.ScheduledDate = start;
            Task.Update(taskWithStartDate);
        }

        // Update scheduled dates for dependent tasks
        foreach (TaskInList task in tasksWithoutDep)
        {
            updateSceduledDateInDep(task.Id);
        }
    }

    /// <summary>
    /// Updates the scheduled date for tasks dependent on a given task recursively.
    /// </summary>
    /// <param name="id">The ID of the task whose dependents need to have their scheduled dates updated.</param>
    /// <remarks>
    /// This method recursively updates the scheduled date for tasks that depend on the task with the specified ID.
    /// It iterates through all dependent tasks, updating their scheduled dates based on the forecast date of the given task.
    /// If a dependent task already has a scheduled date, it compares the forecast date of the given task with the current scheduled date of the dependent task
    /// and updates the scheduled date if necessary to ensure it reflects the latest possible start date.
    /// </remarks>
    private void updateSceduledDateInDep(int id)
    {
        IEnumerable<TaskInList> dependOnTasks = Task.ReadAll(boTask => boTask.Dependencies!.FirstOrDefault(item => item.Id == id) != null).ToList();
        BO.Task dep = Task.Read(id);
        if (dependOnTasks == null)
            return;
        foreach (TaskInList task in dependOnTasks!)
        {          
            BO.Task taskTODoStartDate = Task.Read(task.Id);
            if (taskTODoStartDate.ScheduledDate is null)
                taskTODoStartDate.ScheduledDate = dep.ForecastDate;
            else
                taskTODoStartDate.ScheduledDate = (dep.ScheduledDate > taskTODoStartDate.ScheduledDate) ? dep.ForecastDate : taskTODoStartDate.ScheduledDate;
            Task.Update(taskTODoStartDate);
          updateSceduledDateInDep(taskTODoStartDate.Id);
        }

    }
}
