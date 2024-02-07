

using BlApi;
using BO;
using System.Collections.Immutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public  DateTime? ProjectStartDate { get => ProjectStartDate; init=>ProjectStartDate=value; }
    public  DateTime? ProjectCompletetDate { get => ProjectCompletetDate; init => ProjectCompletetDate = value; }

    public  ProjectStatus CheckProjectStatus()
    {
        if (ProjectStartDate == null)
            return ProjectStatus.Planing;
        if(Task.ReadAll(item=>item.ScheduledDate is null)is null)         
            return ProjectStatus.Execution;
        return ProjectStatus.Mid;
    }

    public void CreateSchedule()
    {
        //הנחות: יש לנו תאריך התחלה של הפרוייקט
        //יש לכל משימה משך זמן ורמת מורכבות
        //יש תלויות
        //צריך לעדכן לכל משימה את  הscheduled date
        DateTime start = ProjectStartDate!.Value;
        IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies == null);
        foreach(TaskInList task in tasksWithoutDep)
        {
            BO.Task taskWithStartDate = Task.Read(task.Id);
            taskWithStartDate.ScheduledDate = start;
            Task.Update(taskWithStartDate); 
        }

        foreach (TaskInList task in tasksWithoutDep)
        {
            updateSceduledDateInDep(task.Id);
        }


    }

    private void updateSceduledDateInDep(int id)
    {
        //id = id of task hat other tasks depends on her
        IEnumerable<TaskInList> dependOnTasks = Task.ReadAll(boTask => boTask.Dependencies!.FirstOrDefault(item => item.Id == id) != null);
        BO.Task dep = Task.Read(id);
        if (dependOnTasks != null)
            return;
        foreach (TaskInList task in dependOnTasks!)
        {          
            BO.Task taskTODoStartDate = Task.Read(task.Id);
            if (taskTODoStartDate.ScheduledDate is null)
                taskTODoStartDate.StartDate= dep.ForecastDate;
            else
                taskTODoStartDate.StartDate=(dep.ScheduledDate > taskTODoStartDate.ScheduledDate) ? dep.ScheduledDate : taskTODoStartDate.ScheduledDate;
          updateSceduledDateInDep(taskTODoStartDate.Id);
        }

    }
}
