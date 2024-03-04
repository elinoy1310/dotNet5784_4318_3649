namespace BlApi;
public interface ITask
{
    public IEnumerable<BO.Engineer.TaskInList> ReadAll(Func<BO.Engineer.Task, bool>? filter = null);
    public BO.Engineer.Task Read(int id);
    public BO.Engineer.Task? Read(Func<BO.Engineer.Task, bool> filter);
    public int Create(BO.Engineer.Task task);
    public void Update(BO.Engineer.Task task);
    public void Delete(int id);
    public void CreateStartDate(int id,DateTime date);
}

