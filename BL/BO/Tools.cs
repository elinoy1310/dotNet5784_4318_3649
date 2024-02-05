using System.Reflection;
using System.Runtime.CompilerServices;

namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t.GetType().GetProperties())
        { 
            if (item.GetType() == typeof(IEnumerable<T>))
                str += item.ToStringProperty();
            str += "\n" + item.Name + ": " + item.GetValue(t, null);
        }
        return str;
    }

}
