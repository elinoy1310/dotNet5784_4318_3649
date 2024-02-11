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


    public int Create(BO.Engineer engineer)
    {
        //can't add after the project has started
        //without task
        if (engineer.Id <= 0 || engineer.Name == null || engineer.Cost <= 0 || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty))
            throw new BlWrongDataException("Invalid data");
        if (engineer.Task is not null)
            throw new BlWrongDataException("Can't add engineer with task after the project has started");
        DO.Engineer newEngineer = new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost, engineer.Name, (DO.EngineerExperience)engineer.level);
        try
        {
            int idNewEnginerr = _dal.Engineer.Create(newEngineer);
            return idNewEnginerr;
        }

        catch (DO.DalAlreadyExistException ex)
        {
            throw new BlAlreadyExistException($"Engineer with ID={engineer.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        if (createTaskInEngineer(id) is not null)
            throw new BlCannotBeDeletedException("Can't Delete an Engineer who has already finished performing a task or is actively performing a task");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Engineer with ID={id} was not found", ex);
        }

    }

    public BO.Engineer Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
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
    public BO.Engineer Read(Func<BO.Engineer, bool>? filter = null)
    {
        if (filter == null)
            return ReadAll().FirstOrDefault() ?? throw new BlDoesNotExistException("there are 0 engineers right now");
        else
            return ReadAll().FirstOrDefault(filter) ?? throw new BlDoesNotExistException($"Engineer with {filter} does not exist");
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        var engineers = from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        select Read(doEngineer.Id);
        ///select new BO.Engineer
        ///{
        ///    Id = doEngineer.Id,
        ///    Name = doEngineer.Name,
        ///    Email = doEngineer.Email,
        ///    level = (BO.EngineerExperience)doEngineer.Level,
        ///    Cost = doEngineer.Cost,
        ///    Task = createTaskInEngineer(doEngineer.Id)
        ///};
        if (filter == null)
            return engineers;
        return engineers.Where(filter);
    }

    private void updatePreviousTask(int idTask)
    {
        _dal.Task.Update(_dal.Task.Read(idTask)! with { CompleteDate = DateTime.Now, EngineerId = null });
    }
    private void updateNewTask(int idTask,int idEngineer,BO.EngineerExperience engineerLevel)
    {
        DO.Task t = _dal.Task.Read(idTask)!;
        if (t.ScheduledDate > DateTime.Now)
            throw new BlCannotBeUpdatedException("the task can't be started");
        if ((int)t.Complexity > (int)engineerLevel)
            throw new BlCannotBeUpdatedException("the task's level is beyond the engineer's level");
        if (t.EngineerId == null)
            _dal.Task.Update(t with { EngineerId = idEngineer, StartDate = DateTime.Now });
        else
            throw new BlCannotBeUpdatedException("other engineer take care of the new engineer's task");
    }
    public void Update(BO.Engineer engineer)
    {
        BO.Engineer toUpdateEngineer = Read(engineer.Id);
        if (engineer.Name == null || !System.Net.Mail.MailAddress.TryCreate(engineer.Email, out System.Net.Mail.MailAddress? empty) || engineer.level < toUpdateEngineer.level || engineer.Cost <= 0)
            throw new BlWrongDataException("Invalid data");

        /// if(BO.TaskImplementation.Read(toUpdateEngineer.Task.Id).status=mid)
        ///שינוי משימה רק אחרי לוז
        ///המשימה שונה ממה שכבר יש לו-1
        ///מראש עבד על משהו ואז אמרו לו נל
        ///עדכון תאריל המטלה הקודמת של הסיום
        /// לשים את הIג  של המהנדס מטלה אחרת ממטלה קודמת לעדכן קודמת בתאריך סיום ולמטלה חדשה לבדוק 
        ///לאחר בדיקות שאין לו מהנדס 
        if (engineer.Task != toUpdateEngineer.Task)
        {
            if (engineer.Task == null)
                updatePreviousTask(toUpdateEngineer.Task!.Id);

            else if (toUpdateEngineer.Task == null)
                updateNewTask(engineer.Task!.Id, engineer.Id,engineer.level);
          
            ///if both not null
            ///check new task scheduled date is after now
            ///update id engineer previous task+complete date=now
            ///update  new task .engineerid =engineer.id +start date=now
            ///send new engineer to the dal
            else
            {
                updateNewTask(engineer.Task!.Id,engineer.Id,engineer.level);
                updatePreviousTask(toUpdateEngineer.Task!.Id);
            }
        }
       
        try
        {
            DO.Engineer updatedEngineer = new DO.Engineer(engineer.Id, engineer.Email, engineer.Cost, engineer.Name, (DO.EngineerExperience)engineer.level);
            _dal.Engineer.Update(updatedEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Engineer with ID={engineer.Id} was not found", ex);
        }
    }

    private TaskInEngineer? createTaskInEngineer(int id)
    {
        var task = _dal.Task.ReadAll().FirstOrDefault(item => item is not null && item.EngineerId == id);
        if (task is not null)
            return new TaskInEngineer { Id = task.Id, Alias = task.Alias };
        return null;
    }
}
