using System.Collections.Generic;
using System;
using System.Collections;

namespace PL;
internal class LevelCollection : IEnumerable
{
    static readonly IEnumerable<BO.EngineerExperience> s_enums =(Enum.GetValues(typeof(BO.EngineerExperience)) as IEnumerable<BO.EngineerExperience>)!;
    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}

