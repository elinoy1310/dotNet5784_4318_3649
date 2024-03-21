
namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;



internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// Checks that the item is not found and adds it to the list
    /// </summary>
    /// <param name="item">The item we want to add to the list</param>
    /// <returns></returns>
    /// <exception cref="Exception">An exception is thrown if the item is in the list</exception>
    public int Create(Engineer item)
    {
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// <summary>
    /// Deleting an existing object with a certain ID from the list
    /// </summary>
    /// <param name="id">The ID of the object we want to delete</param>
    /// <exception cref="Exception">Throwing an exception if the ID we were looking for is not found</exception>
    public void Delete(int id)
    {
        Engineer? found = Read(id);
        if (found != null)
        {
            DataSource.Engineers.Remove(found);

            return;
        }
        throw new DalDoesNotExistException($"Engineer with ID = {id} was not found");
    }

    public void DeleteAll()
    {
        DataSource.Engineers.Clear();
    }

    /// <summary>
    /// Return if the sent ID is in the list
    /// </summary>
    /// <param name="id">The ID of the object we want to find</param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(eng => eng?.Id == id);
    }

    /// <summary>
    /// Reads engineers from the data source based on the provided filter.
    /// </summary>
    /// <param name="filter">
    /// A filter function to apply when retrieving engineers. If null, returns the first engineer without any filter.
    /// </param>
    /// <returns>
    /// The first engineer from the data source that satisfies the filter condition. Returns null if the filter is null.
    /// </returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        if (filter == null)
            return DataSource.Engineers.FirstOrDefault();
        return DataSource.Engineers.FirstOrDefault(filter!);
    }

    /// <summary>
    /// Returning a copy of the list of references to all objects of type Dependency
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">The object with the updated details</param>
    public void Update(Engineer item)
    {
        Delete(item.Id);
        Create(item);
    }
}
