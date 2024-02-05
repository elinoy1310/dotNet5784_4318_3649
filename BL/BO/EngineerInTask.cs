namespace BO;

public class EngineerInTask
{
    public int Id { get; init; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();

}
