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
    static readonly IEnumerable<BO.Engineer.EngineerExperience> s_enums =(Enum.GetValues(typeof(BO.Engineer.EngineerExperience)) as IEnumerable<BO.Engineer.EngineerExperience>)!;

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
