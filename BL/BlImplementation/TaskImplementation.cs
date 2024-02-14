﻿using BlApi;
using BO;

namespace BlImplementation;
//להוסיף משתנה בIBL שיגיד לי באיזה שלב אני נמצאת בפרוייקט

//פונקצייה יצירת לוז אוטומוטי שתהיה ציבורית ותהיה בIBL 
//ואם אני רוצה שתופעל אוטומטית זה פרטי

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private BlApi.IBl _bl = BlApi.Factory.Get();
    //לא לאפשר הכנסת מהנדס למשימה
    public int Create(BO.Task task)
    {
        if (_bl.CheckProjectStatus() == BO.ProjectStatus.Execution)
            throw new BlWrongDataException("Cannot add a task when the project has started");
        if (task.Id >= 0 && task.Alias != null)
        {
            int? engineerId = task.Engineer is not null ? task.Engineer.Id : null;
            DO.Task doTask = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, engineerId,(DO.EngineerExperience)task.Complexity);
            int idNewTask = _dal.Task.Create(doTask);
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

    public void CreateStartDate(int id, DateTime date)
    {
        List <TaskInList> list = returnDepTask(id).ToList();
        var tempListStatus = list.Select(task => task.Status == Status.Unscheduled).ToList();
        if (tempListStatus != null)
            throw new BlNotUpdatedDataException($"Not all start dates of previous tasks of task with ID={id} updates");
        var findTask = from taskInAllTasks in _dal.Task.ReadAll()
                       from depTask in list
                       where taskInAllTasks.Id == depTask.Id
                       select taskInAllTasks;
        var tempListDate = from task in findTask
                           where date < task.CompleteDate
                           select task;
        if (tempListDate!=null)
            throw new BlWrongDataException($"The date is earlier than all the estimated end dates of the previous tasks for the task with ID={id}");
        DO.Task updateTask = _dal.Task.Read(id) ?? throw new NotImplementedException();
        _dal.Task.Update(updateTask with {ScheduledDate=date});
    }

    public void Delete(int id)
    {
        // אי אפשר למחוק אחרי יצירת לוז
        if (_bl.CheckProjectStatus()==BO.ProjectStatus.Execution)
            throw new BlCannotBeDeletedException($"Task with ID={id} cannot be deleted after the project started");
        BO.Task DelTask = Read(id);
        if (DelTask == null || _dal.Dependency.ReadAll().FirstOrDefault(d => d?.DependsOnTask == id) != null) 
            throw new BlCannotBeDeletedException($"Task with ID={id} cannot be deleted");
        _dal.Task.Delete(DelTask.Id);
    }

    public BO.Task Read(int id)
    {
        DO.Task? taskRead = _dal.Task.Read(id);
        if (taskRead == null)
            throw new BlDoesNotExistException($"Task with ID={id} does Not exist");
        return converFromDOtoBO(taskRead);
    }

    public BO.Task? Read(Func<BO.Task, bool> filter)//עדכון NAMEengineer
    {
        IEnumerable <BO.Task> convertAll= from item in _dal.Task.ReadAll()
                                          let convertItem = converFromDOtoBO(item)
                                          select convertItem;
        if (filter != null)
             convertAll.FirstOrDefault(filter);
        return convertAll.FirstOrDefault();
    }


    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null) //לא מדפיס הכל ולא לזרוק שגיאה אם לא מוצא את המהנדס
    {
        if (filter!=null)
        {
            IEnumerable<BO.Task> isFilter = from item in _dal.Task.ReadAll()
                                            let convertItem = converFromDOtoBO(item)
                                            where filter(convertItem)
                                            select convertItem;
            return (from item in isFilter 
                    select new BO.TaskInList { Id = item.Id, Alias = item.Alias, Description = item.Description, Status = item.Status });
        }
        else
            return (from item in _dal.Task.ReadAll() 
                    let convertItem = converFromDOtoBO(item)
                    select new BO.TaskInList { Id = convertItem.Id, Alias = convertItem.Alias, Description = convertItem.Description, Status = convertItem.Status });
    }

    //אי אפשר לעדכן דברים שמשפיעים על הלוז כמו משך הזמן את הסצדולד רק ב שלב הביניים של הפרויקט אי אפשר לעדכן את הENGINEERID
    //אי אפשר להוסיף משימות בשלב הביצוע ,אי אפשר לעדכן תלויות בשלב הביצוע
    // בשלב התכנון- לעדכן רק משך זמן נדרש רמת קושי ותלויות
    //מחיקת משימות אסורה בשלב הביצוע 
    public void Update(BO.Task task)//עדכון הID רק אם המהנדס ברמה
    {
        //if (task.Engineer != null)
        //{
        //    DO.Engineer? engInTask = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.Engineer?.Id);
        //    if (engInTask == null)
        //        throw new BlDoesNotExistException($"Engineer with ID={task.Engineer?.Id} was not found");
        //    if ((int)engInTask.Level < (int)task.Complexity)
        //        throw new BlWrongDataException("The level of the engineer is too low for the level of the task");
        //}
        //DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, task.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
        //_dal.Task.Update(convertFromBOtoDO);
        BO.Task Originaltask = Read(task.Id);
        if (_bl.CheckProjectStatus() == BO.ProjectStatus.Planing)
        {
            updateDependencies(task);
            {
                DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, Originaltask.CreatedAtDate, Originaltask.ScheduledDate, Originaltask.StartDate, Originaltask.CompleteDate, null, task.Deliverables, task.Remarks, Originaltask.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
                _dal.Task.Update(convertFromBOtoDO);
            }
        }
        else if (_bl.CheckProjectStatus() == BO.ProjectStatus.Mid)
        {
            CheckingEngineer(task);
            updateDependencies(task);
            DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, task.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
            _dal.Task.Update(convertFromBOtoDO);
        }
        else if (_bl.CheckProjectStatus() == BO.ProjectStatus.Execution)
        {
            CheckingEngineer(task);
            DO.Task convertFromBOtoDO = new DO.Task(task.Id, task.Alias, task.Description, false, task.RequiredEffortTime, task.CreatedAtDate, task.ScheduledDate, task.StartDate, task.CompleteDate, null, task.Deliverables, task.Remarks, task.Engineer?.Id, (DO.EngineerExperience)task.Complexity);
            _dal.Task.Update(convertFromBOtoDO);
        }

    }

    private void CheckingEngineer(BO.Task task)
    {
        if (task.Engineer != null)
        {
            DO.Engineer? engInTask = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.Engineer?.Id);
            if (engInTask == null)
                throw new BlDoesNotExistException($"Engineer with ID={task.Engineer?.Id} was not found");
            if ((int)engInTask.Level < (int)task.Complexity)
                throw new BlWrongDataException("The level of the engineer is too low for the level of the task");
        }
    }

    private void updateDependencies(BO.Task task)
    {
        if (task.Dependencies is not null)
        {
            foreach (var d in task.Dependencies)
            {
                _dal.Dependency.Create(new DO.Dependency(0, task.Id, d.Id));
            }
        }
    }
    private Status statusCalculation(DO.Task task)
    {
        Status status = 0;
        if (task.ScheduledDate == null)
            status = Status.Unscheduled;
        else if (task.StartDate == null)
            status = Status.Scheduled;
        else if (task.CompleteDate == null)
            status = Status.OnTrack;
        else
            status = Status.Done;
        return status;
    }

    public IEnumerable<TaskInList> returnDepTask(int id)
    {
        List<TaskInList> depTaskInLists = new List<TaskInList>();
        //var depGroup = from dep in _dal.Dependency.ReadAll()
        //               group dep by dep.Dependent into d
        //               select d;
        //foreach (var item in depGroup)
        //{
        //    if (item.Key == id)
        //    {
        //        foreach (var d in item)
        //        {
        //            DO.Task task1depTask = _dal.Task.ReadAll().FirstOrDefault(t => t?.Id == d.DependsOnTask) ?? throw new BlDoesNotExistException($"Task with ID={d.DependsOnTask} does Not exist");
        //            depTaskInLists.Add(new TaskInList { Id = task1depTask.Id, Alias = task1depTask.Alias, Description = task1depTask.Description, Status = statusCalculation(task1depTask) });
        //        }
        //    }
        //}
        var depList = from dep in _dal.Dependency.ReadAll()
                      where (dep.Dependent == id)
                      select dep.DependsOnTask;
        foreach (var item in depList)
        {
            DO.Task depTask = _dal.Task.ReadAll().FirstOrDefault(t => t?.Id == item) ?? throw new BlDoesNotExistException($"Task with ID={item} does Not exist");
            depTaskInLists.Add(new TaskInList { Id = depTask.Id, Alias = depTask.Alias, Description = depTask.Description, Status = statusCalculation(depTask) });
        }
        return depTaskInLists;
    }


    private EngineerInTask returnEngineerOnTask(DO.Task task)
    {
        if (task.EngineerId == null)
            return null!;
        else
        {
            DO.Engineer eng = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.EngineerId) ?? throw new BlDoesNotExistException($"Engineer with ID={task.EngineerId} does Not exist");
            return new EngineerInTask { Id = eng.Id, Name = eng.Name };
        }
    }

    private DateTime? returnForecastDate (DateTime? scheduledDate, DateTime? StartDate, TimeSpan? RequiredEffortTime)
    {
        if (StartDate==null)
            return (scheduledDate + RequiredEffortTime);
        return ((scheduledDate > StartDate) ? scheduledDate : StartDate) + RequiredEffortTime;
    }

    private BO.Task converFromDOtoBO(DO.Task task)
    {
        BO.Task newTask=
         new BO.Task()
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
            Complexity = (BO.EngineerExperience)task.Complexity
        };
        return newTask;
    }
}
