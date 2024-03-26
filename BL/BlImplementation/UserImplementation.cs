

using BlApi;
using BO;
using DalApi;
using DO;

namespace BlImplementation;

internal class UserImplementation : BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    // private BlApi.IBl _bl = BlApi.Factory.Get();
    public int Create(BO.User user)
    {

        try
        {
            DO.User newUser = new DO.User(user.UserId, (DO.UserType)user.UserType, user.passWord);
            // Attempt to create the user in the data layer
            int idNewUser = _dal.User.Create(newUser);
            return idNewUser;
        }

        catch (DO.DalAlreadyExistException ex)
        {
            // Handle the case where an engineer with the same ID already exists
            throw new BlAlreadyExistException($"User with user name={user.UserId} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            _dal.User.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"User with user name={id} does Not exist", ex);
        }
    }

    public BO.User Read(int id)
    {
        // Retrieve the user details from the data layer
        DO.User? doUser = _dal.User.Read(id);
        // Check if the engineer exists
        if (doUser == null)
            throw new BlDoesNotExistException($"User with user name={id} does Not exist");
        // Create and return the business object representation of the engineer
        return new BO.User
        {
            UserId = doUser.userId,
            UserType = (BO.UserType)doUser.type,
            passWord = doUser.password,
        };
    }

    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        var users = from DO.User user in _dal.User.ReadAll()
                    select Read(user.userId);
        if (filter == null)
            return users;
        else
            return users.Where(filter);
    }

    public BO.UserType ReadType(int id, string password)
    {
        try
        {
            if (Read(id).passWord == password)
                return Read(id).UserType;
            else
                throw new BlWrongDataException("Wrong password,try again");
        }
        catch(BO.BlDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"User with user name={id} does Not exist", ex);
        }

    }

    public void Update(BO.User user)
    {
        try
        {
            if (user.passWord is null)
                throw new BlCannotBeUpdatedException("user must have password");
            DO.User? updateUser = new DO.User(user.UserId, (DO.UserType)user.UserType, user.passWord);
            _dal.User.Update(updateUser);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"User with user name={user.UserId} does Not exist", ex);
        }
    }
   
}
