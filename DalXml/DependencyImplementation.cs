
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

internal class DependencyImplementation:IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    /// <summary>
    /// The function automatically creates a new Id and adds the item with the updated Id to the XML file
    /// </summary>
    /// <param name="item">The item we want to add to the XML file</param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        int newId = Config.NextDependencyId;//create new id
        Dependency copyItem = item with { Id = newId };//copy the item and change the id
        dependencies.Add(copyItem);//adds to the list
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencys_xml);//save the list in XML file
        return newId;
    }

    /// <summary>
    /// Deleting an existing object with a certain ID from the XML file
    /// </summary>
    /// <param name="id">The ID of the object we want to delete</param>
    /// <exception cref="Exception">Throwing an exception if the ID we were looking for is not found</exception>
    public void Delete(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        if (dependencies.RemoveAll(x => x.Id == id) == 0)
            throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist"); //if the item is not in the list           
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencys_xml);//save the list in XML file
    }

    public void DeleteAll()
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        dependencies.Clear();
        XMLTools.SaveListToXMLSerializer(dependencies, s_dependencys_xml);
    }

    /// <summary>
    /// Return if the sent ID is in the XML file
    /// </summary>
    /// <param name="id">The ID of the object we want to find</param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        return dependencies.FirstOrDefault(x => x?.Id == id);
    }

    /// <summary>
    /// Reads dependencies from the XML file based on the provided filter.
    /// </summary>
    /// <param name="filter">
    /// A filter function to apply when retrieving dependencies. If null, returns the first dependency without any filter.
    /// </param>
    /// <returns>
    /// The first dependency from the XML file that satisfies the filter condition. Returns null if the filter is null.
    /// </returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        if(filter is null)
            return dependencies.FirstOrDefault();
        return dependencies.FirstOrDefault(filter!);
    }

    /// <summary>
    /// Returning a copy of the collection of references to all objects of type Dependency based on filter
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        if (filter != null)
        {
            return from item in dependencies
                   where filter(item)
                   select item;
        }
        return from item in dependencies
               select item;
    }

    /// <summary>
    /// Update of an existing object
    /// </summary>
    /// <param name="item">The object with the updated details</param>
    public void Update(Dependency item)
    {
        List<Dependency> dependencies = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);//imports the data from the XML file into a list
        Delete(item.Id);
        dependencies.Add(item);
        XMLTools.SaveListToXMLSerializer<Dependency>(dependencies, s_dependencys_xml);//save the list in XML file
    }
}
