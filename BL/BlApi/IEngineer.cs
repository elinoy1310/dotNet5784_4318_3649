
using DO;
using System.ComponentModel;

namespace BlApi;

public interface IEngineer
{
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter=null);
    public BO.Engineer Read(int id);
    public void Add(BO.Engineer engineer);
    public void Delete(int id);

    public void Update(BO.Engineer engineer);

}
 