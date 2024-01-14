namespace DO;

[Serializable]
public class DalDoesNotExistException : Exception
{
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class DalAlreadyExistException : Exception
{
    public DalAlreadyExistException(string? message) : base(message) { }
}

[Serializable]
public class DalWrongInputFormatException : Exception
{
    public DalWrongInputFormatException(string? message) : base(message) { }
}

[Serializable]
public class DalCanNotBeNullExistException : Exception
{
    public DalCanNotBeNullExistException(string? message) : base(message) { }
}



