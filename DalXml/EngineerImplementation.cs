namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class EngineerImplementation:IEngineer
{
    readonly string s_engineers_xml = "engineers";

    /// <summary>
    /// Adds a new Engineer to the XML file.
    /// </summary>
    /// <param name="item">The Engineer object to be added.</param>
    /// <returns>The ID of the newly created Engineer.</returns>
    /// <exception cref="DalAlreadyExistException">
    /// Thrown if an Engineer with the same ID already exists in the XML file.
    /// </exception>
    public int Create(Engineer item)
    {
        // Load the existing Engineers from the XML file
        XElement? EngElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        // Check if an Engineer with the same ID already exists
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistException($"Engineer with ID={item.Id} already exists");
        // Create a new XElement representing the new Engineer
        XElement? ElemItem = new XElement("Engineer", new XElement("Id", item.Id), new XElement("Email", item.Email), new XElement("Cost", item.Cost), new XElement("Name", item.Name), new XElement("Level", item.Level));
        // Add the new Engineer's XElement to the existing Engineers
        EngElem.Add( ElemItem );
        // Save the updated list of Engineers back to the XML file
        XMLTools.SaveListToXMLElement(EngElem, s_engineers_xml);
        // Return the ID of the newly created Engineer
        return item.Id;
    }

    /// <summary>
    /// Deletes an Engineer from the XML file based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to be deleted.</param>
    public void Delete(int id)
    {
        // Load the existing Engineers from the XML file
        XElement? EngElem = XMLTools.LoadListFromXMLElement(s_engineers_xml);
        // Find the XElement corresponding to the Engineer with the specified ID
        XElement? ElemDelItem = EngElem.Elements().FirstOrDefault(eng => (int?)eng.Element("Id") == id);
        // If the XElement is found, remove it from the XML structure
        if (ElemDelItem != null)
                ElemDelItem.Remove();
        // Save the updated list of Engineers back to the XML file
        XMLTools.SaveListToXMLElement(EngElem, s_engineers_xml);
    }

    /// <summary>
    /// Reads and retrieves an Engineer from the XML file based on the provided ID.
    /// </summary>
    /// <param name="id">The ID of the Engineer to be retrieved.</param>
    /// <returns>
    /// An Engineer object if found; otherwise, returns <c>null</c> if no Engineer with the specified ID exists.
    /// </returns>
    public Engineer? Read(int id)
    {
        // Load the existing Engineers from the XML file
        XElement? engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault(eng => (int?)eng.Element("Id") == id);
        // If the XElement is null, no matching Engineer was found and Convert the XElement to an Engineer object and return it
        return engineerElem is null? null : getEngineer(engineerElem);   
    }

    /// <summary>
    /// Reads and retrieves an Engineer from the XML file based on the provided filter function.
    /// </summary>
    /// <param name="filter">A filter function to match specific criteria for an Engineer.</param>
    /// <returns>
    /// An Engineer object that matches the specified criteria;
    /// otherwise, returns <c>null</c> if no matching Engineer is found.
    /// </returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        // Load the existing Engineers from the XML file, convert XElement to Engineer, and apply the filter
        return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(en => getEngineer(en)).FirstOrDefault(filter);    
    }

    /// <summary>
    /// Reads and retrieves all Engineers from the XML file, optionally applying a filter.
    /// </summary>
    /// <param name="filter">
    /// An optional filter function to include only Engineers that match specific criteria.
    /// </param>
    /// <returns>
    /// An IEnumerable collection of Engineer objects that match the specified criteria
    /// or all Engineers if no filter is provided.
    /// </returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        // Load the existing Engineers from the XML file, convert XElement to Engineer, and optionally apply the filter
        if (filter==null)
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e=>getEngineer(e));
        else
            return XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().Select(e=>getEngineer(e)).Where(filter); 
    }

    /// <summary>
    /// Updates an existing Engineer in the XML file or creates a new one if not found.
    /// </summary>
    /// <param name="item">The Engineer object containing updated information.</param>
    public void Update(Engineer item)
    {
        // Delete the existing Engineer with the specified ID
        Delete(item.Id);
        // Create or update the Engineer with the provided information
        Create(item);
    }

    /// <summary>
    /// Converts an XElement representing an Engineer to an Engineer object.
    /// </summary>
    /// <param name="e">The XElement representing an Engineer in XML.</param>
    /// <returns>The converted Engineer object.</returns>
    private static Engineer getEngineer(XElement e)
    {
        return new Engineer()
        {
            Id = e.ToIntNullable("Id") ?? throw new FormatException("can't  convert id"),
            Email = (string?)e.Element("Email") ?? null,
            Cost = e.ToDoubleNullable("Cost") ?? throw new FormatException("can't  convert id"),
            Name = (string?)e.Element("Name") ?? "",
            Level = e.ToEnumNullable<EngineerExperience>("Level") ?? EngineerExperience.Beginner,
        };
    }

    /// <summary>
    /// Deletes all engineers from the XML data source.
    /// </summary>
    public void DeleteAll()
    {
        // Load the list of engineers from the XML file
        XElement? engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault();
        Engineer engineer;
        // Iterate through each engineer element
        while (engineerElem is not null)
        {
            // Retrieve the engineer object from the XML element 
            engineer = getEngineer(engineerElem);
            // Delete the engineer using the provided Delete method
            Delete(engineer.Id);
            // Load the next engineer element for the next iteration
            engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault();
        }

    }

} 


