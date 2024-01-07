

namespace DO;
/// <summary>
/// Dependency Entity
/// </summary>
/// <param name="Id">Personal unique ID of the Dependency</param>
/// <param name="Dependent">Id number of pending task</param>
/// <param name="DependsOnTask">Previous assignment Id number</param>
public record Dependency
(
    int Id,
    int? Dependent=null,
    int? DependsOnTask=null
)
{
    //a parameterizes constructor is automatically present
    public Dependency() : this(0) { } //empty constructor

}
