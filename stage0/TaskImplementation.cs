namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    /// <summary>
    /// creates new Task item with new id and adds it to the tasks's Xml file
    /// </summary>
    /// <param name="item">the item we want to add to the Xml file, item.Id=default value</param>
    /// <returns></returns>
    public int Create(Task item)
    {   
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        int newId = Config.NextTaskId;//create new id
        Task copyItem = item with { Id = newId };//copy the item and change the id
        tasks.Add(copyItem);//adds to the list
        XMLTools.SaveListToXMLSerializer<Task>(tasks, s_tasks_xml);//save the list in XML file
        return newId;
    }


    /// <summary>
    /// searches the item in tasks according to id and remove it from the Xml file, if the item doesn't exists throw exception
    /// </summary>
    /// <param name="id">id of the item we want to delete</param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        if(tasks.RemoveAll(x=>x.Id == id)==0)
            throw new DalDoesNotExistException($"Task with ID={id} does Not exist"); //if the item is not in the list           
        XMLTools.SaveListToXMLSerializer<Task>(tasks, s_tasks_xml);//save the list in XML file
    }

    /// <summary>
    /// search the item in the Xml file with the received id, if not found returns null
    /// </summary>
    /// <param name="id">The ID of the object we want to find</param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        return tasks.FirstOrDefault(x => x?.Id == id);
    }

    /// <summary>
    /// Reads tasks from the Xml file based on the provided filter.
    /// </summary>
    /// <param name="filter">
    /// A filter function to apply when retrieving tasks. If null, returns the first task without any filter.
    /// </param>
    /// <returns>
    /// The first task from the Xml file that satisfies the filter condition. Returns null if the filter is null.
    /// </returns>
    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        if (filter == null)
            return tasks.FirstOrDefault();
        return tasks.FirstOrDefault(filter!);
    }

    /// <summary>
    /// returns list with all the Task item in the tasks's Xml file
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        if (filter != null)
        {
            return from item in tasks
                   where filter(item)
                   select item;
        }
        return from item in tasks
               select item;
    }

    /// <summary>
    /// deletes the old item (according to item.id) from the tasks's Xml file and adds item to the Xml file
    /// </summary>
    /// <param name="item">Task item with updated values </param>
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);//imports the data from the XML file into a list
        Delete(item.Id);
        tasks.Add(item);
        XMLTools.SaveListToXMLSerializer<Task>(tasks, s_tasks_xml);//save the list in XML file
    }

    /// <summary>
    /// Deletes all tasks from the XML data source.
    /// </summary>
    public void DeleteAll()
    {
        // Load the list of tasks from the XML file using XML serialization
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        // Clear the list of tasks
        tasks.Clear();
        // Save the empty list back to the XML file using XML serialization
        XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
    }
}
