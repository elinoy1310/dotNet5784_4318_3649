


using BlApi;
using BO;




namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void Add(BO.Task task)
    {
        if (task.Id > 0 && task.Alias != null)
        {
            DO.Task doTask = new DO.Task(task.Id, task.Alias, task.Description );
            throw new NotImplementedException();
        }
    }

    public void CreateStartDate(int id, DateTime date)
    {

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        BO.Task DelTask = Read(id);
        if (DelTask == null || isPreviousTask(DelTask.Id) == true)
            throw new NotImplementedException();
        _dal.Task.Delete(DelTask.Id);
    }

    public BO.Task Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new NotImplementedException();
        return converFromDOtoBO(task);
    }

    public IEnumerable<BO.Task> ReadAll(Func<System.Threading.Tasks.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
                select converFromDOtoBO(doTask));
    }

    public void Update(BO.Task task)
    {
        BO.Task updateTask = Read(task.Id);
        if (updateTask == null)
            throw new NotImplementedException();
        DO.Task convertFromBOtoDO = new DO.Task(updateTask.Id, updateTask.Alias, updateTask.Description, false, updateTask.RequiredEffortTime, updateTask.CreatedAtDate, updateTask.ScheduledDate, updateTask.StartDate, updateTask.CompleteDate, null, updateTask.Deliverables, updateTask.Remarks, updateTask.Engineer?.Id, (DO.EngineerExperience)updateTask.Complexity);
        _dal.Task.Update(convertFromBOtoDO);
    }

    private Status statusCalculation (DO.Task task)
    {
        Status status= 0;
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

    private List<TaskInList> returnDepTask(int id)
    {
        List<TaskInList> depTaskInLists= new List<TaskInList>();
        var depGroup = from dep in _dal.Dependency.ReadAll()
                       group dep by dep.Dependent into d
                       select d;
        foreach(var item in depGroup)
        {
            if (item.Key == id)
            {
                foreach (var d in item)
                {
                    DO.Task task1depTask = _dal.Task.ReadAll().FirstOrDefault(t => t?.Id == d.DependsOnTask) ?? throw new NotImplementedException();
                    depTaskInLists.Add(new TaskInList { Id = task1depTask.Id, Alias = task1depTask.Alias, Description=task1depTask.Description, Status=statusCalculation(task1depTask)});
                }
            }
        }
        return depTaskInLists;
    }

    private bool isPreviousTask(int id)
    {
        var depGroup = from dep in _dal.Dependency.ReadAll()
                       orderby dep.Dependent 
                       group dep by dep.Dependent into d
                       select d;
        foreach(var item in depGroup)
            if (item.Key == id)
                return true;
        return false;
    }

    private EngineerInTask returnEngineerOnTask(DO.Task task)
    {
        DO.Engineer eng = _dal.Engineer.ReadAll().FirstOrDefault(e => e?.Id == task.EngineerId)??throw new NotImplementedException();
        return new EngineerInTask { Id = eng.Id, Name = eng.Name };
    }

    private BO.Task converFromDOtoBO(DO.Task task)
    {
        return new BO.Task
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
            ForecastDate = (task.ScheduledDate > task.StartDate) ? task.ScheduledDate : task.StartDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = returnEngineerOnTask(task),
            Complexity = (BO.EngineerExperience)task.Complexity
        };
    }
}
