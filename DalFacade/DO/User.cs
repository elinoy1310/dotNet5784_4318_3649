namespace DO;
public record User
(
    int userId,
    DO.UserType type = UserType.Engineer,
    string? password = null
)
{
    public User() : this(0) { }
}
