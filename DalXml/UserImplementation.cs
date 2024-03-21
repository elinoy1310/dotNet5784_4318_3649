
using DalApi;
using DO;
using System.Linq;
using System.Threading.Tasks;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";
    public int Create(User item)
    {
        if (Read(item.userId) is not null)
            throw new DalAlreadyExistException($"User with ID={item.userId} already exists");
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);//imports the data from the XML file into a list
        users.Add(item);//adds to the list
        XMLTools.SaveListToXMLSerializer<User>(users, s_users_xml);//save the list in XML file
        return item.userId;
    }

    public void Delete(int id)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);//imports the data from the XML file into a list
        if (users.RemoveAll(x => x.userId == id) == 0)
            throw new DalDoesNotExistException($"User with ID={id} does Not exist"); //if the item is not in the list           
        XMLTools.SaveListToXMLSerializer<User>(users, s_users_xml);//save the list in XML file
    }

    public void DeleteAll()
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);//imports the data from the XML file into a list
        users.Clear();         
        XMLTools.SaveListToXMLSerializer<User>(users, s_users_xml);//save the list in XML file
    }

    public User? Read(int id)
    {
       return XMLTools.LoadListFromXMLSerializer<User>(s_users_xml).FirstOrDefault(x => x?.userId == id);
    }

    public User? Read(Func<User, bool> filter)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);//imports the data from the XML file into a list
        if (filter == null)
            return users.FirstOrDefault();
        return users.FirstOrDefault(filter!);
    }

    public IEnumerable<User?> ReadAll(Func<User, bool>? filter = null)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);//imports the data from the XML file into a list
        if (filter != null)
        {
            return from item in users
                   where filter(item)
                   select item;
        }
        return from item in users
               select item;
    }

    public void Update(User item)
    {
        Delete(item.userId);
        Create(item);
    }
}
