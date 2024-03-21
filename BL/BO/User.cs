namespace BO;

public class User
{
    //int userId,
    //DO.UserType type = UserType.Engineer,
    //string? password = null
    public int UserId { get; init; }
    public BO.UserType UserType { get; set; }
    public string? passWord { get; set; }
    public override string ToString() => this.ToStringProperty();
}
