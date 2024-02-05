using BlApi;
using BO;
using DO;
using System.Security.Cryptography;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Add(BO.Engineer engineer)
    {
        if (engineer.Id <= 0 || engineer.Name == null || engineer.Cost <= 0 || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty))
            throw new Exception("Invalid data");
        DO.Engineer newEngineer=new DO.Engineer(engineer.Id,engineer.Email,engineer.Cost,engineer.Name,(DO.EngineerExperience)engineer.level);
        try
        {
            int idNewEnginerr = _dal.Engineer.Create(newEngineer);
            return idNewEnginerr;
        }
        catch (DO.DalAlreadyExistException ex)
        {
            throw new Exception/*BO.BlAlreadyExistsException*/($"Engineer with ID={engineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        BO.Engineer boEngineer=Read(id);
        if (boEngineer!.Task is not null)
            throw new Exception("Can't Delete an Engineer who has already finished performing a task or is actively performing a task");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception/*BO.*/($"Engineer with ID={id} was not found", ex);
        }
          
    }

    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer=_dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new Exception /*BO.BlDoesNotExistException*/($"Engineer with ID={id} does Not exist");
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

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        var tasks= from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
               select Read(doEngineer.Id); ////select new BO.Engineer
               ////{
               ////    Id = doEngineer.Id,
               ////    Name = doEngineer.Name,
               ////    Email = doEngineer.Email,
               ////    level = (BO.EngineerExperience)doEngineer.Level,
               ////    Cost = doEngineer.Cost,
               ////    Task = createTaskInEngineer(doEngineer.Id)
               ////};
         if(filter==null)
            return tasks;
        return tasks.Where(filter);
    }

    public void Update(BO.Engineer engineer)
    {
        BO.Engineer toUpdateEngineer = Read(engineer.Id);
        if (engineer.Name == null || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty)|| engineer.level<toUpdateEngineer.level ||engineer.Cost <= 0)
            throw new Exception("Invalid data");
       // if(BO.TaskImplementation.Read(toUpdateEngineer.Task.Id).status)
       if(engineer.Task is not null)
        {
            //int searchId = toUpdateEngineer.Task!.
            //var tasks = from DO.Task doTask in _dal.Task.ReadAll()
            //            let id = searchId
            //            where doTask.Id == id
            //            select doTask;
            //EngineerInTask newEngineerId=new EngineerInTask() { Id=engineer.Id,Name=engineer.Name };
            DO.Task updateTask = _dal.Task.Read(engineer.Task.Id) ?? throw new Exception($"the Task with id= {engineer.Task.Id} does not exist");
            _dal.Task.Update(updateTask with { EngineerId = engineer.Id });
            if (toUpdateEngineer.Task is not null)
            {
                DO.Task updatePreviousTask = _dal.Task.Read(toUpdateEngineer.Task!.Id)?? throw new Exception($"the Task with id= {engineer.Task.Id} does not exist");
                _dal.Task.Update(updatePreviousTask with { EngineerId = null });
            }
        }
        try
        {
            DO.Engineer updatedEngineer=new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost, engineer.Name, (DO.EngineerExperience)engineer.level);
            _dal.Engineer.Update(updatedEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception/*BO.*/($"Engineer with ID={engineer.Id} was not found", ex);
        }
    }

    private TaskInEngineer? createTaskInEngineer(int id)
    {
       var task=_dal.Task.ReadAll().FirstOrDefault(item => item is not null &&  item.EngineerId== id);
        if (task is not null)
            return new TaskInEngineer { Id = task.Id, Alias = task.Alias };
        return null;
    }

    
}
