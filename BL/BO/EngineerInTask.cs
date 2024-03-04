namespace BO.Engineer;

public class EngineerInTask
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();

}
