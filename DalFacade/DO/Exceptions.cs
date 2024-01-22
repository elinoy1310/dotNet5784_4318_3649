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
public class DalCanNotBeNullException : Exception
{
    public DalCanNotBeNullException(string? message) : base(message) { }
}

public class DalXMLFileLoadCreateException: Exception
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}


