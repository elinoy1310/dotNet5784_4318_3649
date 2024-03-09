using System.Collections.Generic;
using System;
using System.Collections;

namespace PL;

/// <summary>
/// Represents a collection of engineer experience levels.
/// </summary>
internal class LevelCollection : IEnumerable
{
    // Static readonly field containing the enumeration values
    static readonly IEnumerable<BO.EngineerExperience> s_enums =(Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

internal class StatusCollection : IEnumerable
{
    static readonly IEnumerable<BO.Status> s_enums = (Enum.GetValues(typeof(BO.Status)) as IEnumerable<BO.Status>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

//internal class UsersCollection : IEnumerable
//{
//    static readonly IEnumerable<BO.User> s_enums = (Enum.GetValues(typeof(BO.User)) as IEnumerable<BO.User)!;
//    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
//}


