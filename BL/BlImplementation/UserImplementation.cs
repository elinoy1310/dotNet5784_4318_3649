

using BlApi;
using BO;
using DalApi;

namespace BlImplementation;

internal class UserImplementation :BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
   // private BlApi.IBl _bl = BlApi.Factory.Get();
    public int Create(User user)
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
            throw new BlAlreadyExistException($"User with userName={user.UserId} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public User Read(int id)
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

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public UserType ReadType(int id)
    {
        return Read(id).UserType;
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }
}
