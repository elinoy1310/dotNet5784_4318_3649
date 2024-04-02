using System.Xml.Linq;

namespace Dal;

/// <summary>
/// Config helps define the running variables
/// </summary>
internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextDependencyId 
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextDependencyId", value);
    }
    internal static int NextTaskId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);
    }

    public static DateTime? ProjectStartDate
    {
        get
        {
            XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml).Element("StartProject")!;
            if (root.Value == "") return null;
            return DateTime.Parse(root.Value);
        }
        set
        {
            XElement root = XMLTools.LoadListFromXMLElement(s_data_config_xml);
            root.Element("StartProject")!.Value=value.ToString()??"";
            XMLTools.SaveListToXMLElement(root, s_data_config_xml);
        }
    }
    

}
