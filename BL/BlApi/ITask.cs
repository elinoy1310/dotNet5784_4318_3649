namespace BlApi;
public interface ITask
{
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);
    public BO.Task Read(int id);
    public BO.Task? Read(Func<BO.Task, bool> filter);
    public int Create(BO.Task task);
    public void Update(BO.Task task);
    public void Delete(int id);
    public void CreateStartDate(int id,DateTime date);
    public bool PreviousTaskDone(int id);
}

