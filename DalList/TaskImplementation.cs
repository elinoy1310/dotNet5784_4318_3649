namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

internal class TaskImplementation : ITask
{
    /// <summary>
    /// creates new Task item with new id and adds it to the list tasks
    /// </summary>
    /// <param name="item">the item we want to add to the list, item.Id=default value</param>
    /// <returns></returns>
    public int Create(Task item)
    {
        int newId = DataSource.Config.NextTaskId;//create new id
        Task copyItem = item with { Id = newId };//copy the item and change the id
        DataSource.Tasks.Add(copyItem);//adds to the list
        return newId;
    }

    /// <summary>
    /// serches the item in tasks according to id and remove it from the list, if the item doesn't exists throw exception
    /// </summary>
    /// <param name="id">id of the item we want to delete</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Task? foundItem=Read(id);//search the item with the received id
        if (foundItem is not null)  //check if we found the item
        { 
            DataSource.Tasks.Remove(foundItem);
            return;
        }
        throw new DalDoesNotExistException($"Task with ID={id} does Not exist"); //if the item is not in the list
    }

    public void DeleteAll()
    {
       DataSource.Tasks.Clear();
    }

    /// <summary>
    /// search the item in the list with the received id, if not found returns null
    /// </summary>
    /// <param name="id">id of the item we want to delete</param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(x => x?.Id == id);//search the item with the received id
    }

    /// <summary>
    /// Reads tasks from the data source based on the provided filter.
    /// </summary>
    /// <param name="filter">
    /// A filter function to apply when retrieving tasks. If null, returns the first task without any filter.
    /// </param>
    /// <returns>
    /// The first task from the data source that satisfies the filter condition. Returns null if the filter is null.
    /// </returns>
    public Task? Read(Func<Task, bool> filter)
    {
        if (filter == null)
            return DataSource.Tasks.FirstOrDefault(); 
        return DataSource.Tasks.FirstOrDefault(filter!);
    }

    /// <summary>
    /// returns list with all the Task item int the list tasks
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) //stage 2
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }


    /// <summary>
    /// deletes the old item (according to item.id) from the Tasks list and  adds item to the list
    /// </summary>
    /// <param name="item">Task item with updated values </param>
    public void Update(Task item)
    {
       Delete(item.Id);
       DataSource.Tasks.Add(item); //adds the updated item to the list with the same id
    }
}
