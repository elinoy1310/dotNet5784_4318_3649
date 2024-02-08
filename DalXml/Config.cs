namespace Dal;

/// <summary>
/// Config helps define the running variables
/// </summary>
internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    public static DateTime? ProjectStartDate { get; set; }
    public static DateTime? ProjectCompletetDate { get; set; }

}
