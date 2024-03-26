

namespace BlApi;

public interface IUser
{
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null);
    public BO.User Read(int id);
    public BO.UserType ReadType(int id,string password);
    public int Create(BO.User user);
    public void Delete(int id);
    public void Update(BO.User user);
  
}
