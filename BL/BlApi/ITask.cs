namespace BlApi;
public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    public BO.Task Read(int id);
    public int Add(BO.Task task);
    public void Update(BO.Task task);
    public void Delete(int id);
    public void CreateStartDate(int id,DateTime date);
}

