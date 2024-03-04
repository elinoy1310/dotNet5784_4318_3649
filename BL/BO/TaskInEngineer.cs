
namespace BO.Engineer;

public class TaskInEngineer
{
    public int Id { get; init; }
    public string? Alias { get; set; }
    public override string ToString() => this.ToStringProperty();

    public static explicit operator TaskInEngineer(string? v)
    {
        throw new NotImplementedException();
    }
}
