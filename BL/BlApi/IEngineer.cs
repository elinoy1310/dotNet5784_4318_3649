
namespace BlApi;

public interface IEngineer
{
    public IEnumerable<BO.Engineer.Engineer> ReadAll(Func<BO.Engineer.Engineer, bool>? filter=null);
    public BO.Engineer.Engineer Read(int id);
    public BO.Engineer.Engineer Read(Func<BO.Engineer.Engineer, bool>? filter = null);
    public int Create(BO.Engineer.Engineer engineer);
    public void Delete(int id);
    public void Update(BO.Engineer.Engineer engineer);

}
 