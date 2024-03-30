

namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }

}

[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }

}

[Serializable]
public class BlWrongInputFormatException : Exception
{
    public BlWrongInputFormatException(string? message) : base(message) { }
    public BlWrongInputFormatException(string message, Exception innerException)
                : base(message, innerException) { }

}

[Serializable]
public class BlCanNotBeNullException : Exception
{
    public BlCanNotBeNullException(string? message) : base(message) { }
    public BlCanNotBeNullException(string message, Exception innerException)
                : base(message, innerException) { }

}

[Serializable]
public class BlNotUpdatedDataException : Exception
{
    public BlNotUpdatedDataException(string? message) : base(message) { }
}

[Serializable]
public class BlWrongDataException : Exception
{
    public BlWrongDataException(string? message) : base(message) { }
}

[Serializable]
public class BlCannotBeDeletedException : Exception
{
    public BlCannotBeDeletedException(string? message) : base(message) { }
}

[Serializable]
public class BlCannotBeUpdatedException : Exception
{
    public BlCannotBeUpdatedException(string? message) : base(message) { }
}




