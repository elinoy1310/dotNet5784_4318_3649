﻿
namespace Dal;

using System.Collections.Generic;
using DalApi;
using DO;



public class EngrineerImplementation : IEngineer
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
        throw new Exception($"Engineer with ID={item.Id} already exists");
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
        Engineer? found=Read(id);   
        if (found != null) 
        { 
            DataSource.Engineers.Remove(found);
            return;
        }
        throw new Exception($"Engineer with ID = {id} was not found");
    }
    /// <summary>
    /// Return if the sent ID is in the list
    /// </summary>
    /// <param name="id">The ID of the object we want to find</param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        Engineer? found = DataSource.Engineers.Find(eng => eng?.Id == id);
        if (found != null)
            return found;
        else
            return null;
    }
    /// <summary>
    /// Returning a copy of the list of references to all objects of type Dependency
    /// </summary>
    /// <returns></returns>
    public List<Engineer?> ReadAll()
    {
        return new List<Engineer?>(DataSource.Engineers);
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
