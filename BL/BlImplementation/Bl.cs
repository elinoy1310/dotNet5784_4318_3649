

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

    public  ProjectStatus CheckProjectStatus()
    {
        if (ProjectStartDate == null)
            return ProjectStatus.Planing;
        if(Task.ReadAll(item=>item.ScheduledDate is null)is null)         
            return ProjectStatus.Execution;
        return ProjectStatus.Mid;
    }

    public void CreateSchedule(CreateScheduleOption option/*=CreateScheduleOption.Manually*/, int taskId)
    {
        //אופציה 2: המנהל מקיש טאסק 
        //עוברים על כל תאריכי הסיום של המשימות הקודמות ומחזירים את התאריך הכי רחוק
        //אם אין למישמה מסויימת תאריך מתוכנן לסיום אז לזרוק שגיאה 
        switch (option)
        {
            case CreateScheduleOption.Automatically:
                createScheduleAuto();
                break;
            case CreateScheduleOption.Manually:
                createScheduleOptionManually(taskId);
                break;
        }




        ///אופציה א:
        ////הנחות: יש לנו תאריך התחלה של הפרוייקט
        ////יש לכל משימה משך זמן ורמת מורכבות
        ////יש תלויות
        ////צריך לעדכן לכל משימה את  הscheduled date
        //DateTime start = _dal.ProjectStartDate!.Value;
        //IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList();
        //foreach(TaskInList task in tasksWithoutDep)
        //{
        //    BO.Task taskWithStartDate = Task.Read(task.Id);
        //    taskWithStartDate.ScheduledDate = start;
        //    Task.Update(taskWithStartDate); 
        //}

        //foreach (TaskInList task in tasksWithoutDep)
        //{
        //    updateSceduledDateInDep(task.Id);
        //}


    }

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
    }

    private void createScheduleAuto()
    {
        //הנחות: יש לנו תאריך התחלה של הפרוייקט
        //יש לכל משימה משך זמן ורמת מורכבות
        //יש תלויות
        //צריך לעדכן לכל משימה את  הscheduled date
        DateTime start = _dal.ProjectStartDate!.Value;
        IEnumerable<TaskInList> tasksWithoutDep = Task.ReadAll(boTask => boTask.Dependencies?.Count() == 0).ToList();
        foreach (TaskInList task in tasksWithoutDep)
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
