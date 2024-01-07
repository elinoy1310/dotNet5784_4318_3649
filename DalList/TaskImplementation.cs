﻿namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class TaskImplementation : ITask
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
        throw new Exception($"Task with ID={id} does Not exist"); //if the item is not in the list
    }

    /// <summary>
    /// search the item in the list with the received id, if not found returns null
    /// </summary>
    /// <param name="id">id of the item we want to delete</param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(x => x?.Id == id);//search the item with the received id
    }

    /// <summary>
    /// returns list with all the Task item int the list tasks
    /// </summary>
    /// <returns></returns>
    public List<Task?> ReadAll()
    {
        return new List<Task?>(DataSource.Tasks);
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
