

using BlApi;
using BO;

namespace BlImplementation;

internal class UserImplementation : IUser
{
    public int Create(User engineer)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public UserType ReadType(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }
}
