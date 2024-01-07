

namespace DO;

/// <summary>
/// Engineer Entity represents a Engineer with all its props
/// </summary>
/// <param name="Id">Personal unique ID of the Engineer</param>
/// <param name="Email">Private Email of the Engineer</param>
/// <param name="Cost">How much the Engineer get paid per hour</param>
/// <param name="Name">Private Name of the Engineer</param>
/// <param name="Level">Engineer level</param>
public record Engineer
(
    int Id,
    string? Email=null,
    double? Cost=null,
    string? Name=null,
    EngineerExperience Level= EngineerExperience.Beginner
)
{
    //a parameterizes constructor is automatically present
    public Engineer() : this(0) { } //empty constructor
}
