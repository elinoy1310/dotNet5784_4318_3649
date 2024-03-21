
using System.Linq;
using DalApi;
using DO;

namespace Dal;

internal class UserImplementation : IUser
{
    public int Create(User item)
    {
        if (Read(item.userId) is not null)
            throw new DalAlreadyExistException($"User with ID={item.userId} already exists");
        DataSource.Users.Add(item);
        return item.userId;
    }

    public void Delete(int id)
    {
        User? found = Read(id);
        if (found != null)
        {
            DataSource.Users.Remove(found);

            return;
        }
        throw new DalDoesNotExistException($"User with ID = {id} was not found");
    }

    public void DeleteAll()
    {
        DataSource.Users.Clear();
    }

    public User? Read(int id)
    {
        return DataSource.Users.FirstOrDefault(user => user?.userId == id);
    }

    public User? Read(Func<User, bool> filter)
    {
        if (filter == null)
            return DataSource.Users.FirstOrDefault();
        return DataSource.Users.FirstOrDefault(filter!);
    }

    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in DataSource.Users
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Users
               select item;
    }

    public void Update(User item)
    {
        Delete(item.userId);
        Create(item);
    }
}
