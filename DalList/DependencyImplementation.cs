
namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{
    /// <summary>
    /// The function automatically creates a new Id and adds the item with the updated Id to the list
    /// </summary>
    /// <param name="item">The item we want to add to the list</param>
    /// <returns></returns>
    public int Create(Dependency item) 
    {
        int newId = DataSource.Config.NextDependencyId;
        Dependency copy=item with { Id= newId };
        DataSource.Dependencys.Add(copy);   
        return newId;   
    }
    /// <summary>
    /// Deleting an existing object with a certain ID from the list
    /// </summary>
    /// <param name="id">The ID of the object we want to delete</param>
    /// <exception cref="Exception">Throwing an exception if the ID we were looking for is not found</exception>
    public void Delete(int id)
    {
        Dependency? found = Read(id);
        if (found != null)
        {
            DataSource.Dependencys.Remove(found);
            return;
        }
        throw new Exception($"Dependency with ID = {id} does not exist");
    }
    /// <summary>
    /// Return if the sent ID is in the list
    /// </summary>
    /// <param name="id">The ID of the object we want to find</param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        Dependency? found = DataSource.Dependencys.Find(dep => dep?.Id == id);
        if (found != null)
            return found;
        else
            return null;    
    }
    /// <summary>
    /// Returning a copy of the list of references to all objects of type Dependency
    /// </summary>
    /// <returns></returns>
    public List<Dependency?> ReadAll()
    {
        return new List<Dependency?>(DataSource.Dependencys);
    }
    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">The object with the updated details</param>
    public void Update(Dependency item)
    {
        Delete(item.Id);    
        DataSource.Dependencys.Add(item);   
    }
}
