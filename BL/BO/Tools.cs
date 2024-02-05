using System.Reflection;
using System.Runtime.CompilerServices;

namespace BO;

public static class Tools
{
    public static string ToStringProperty<T>(this T t)
    {
        string str = "";
        foreach (PropertyInfo item in t!.GetType().GetProperties())
        {
            str += "\n" + item.Name + ": " + item.GetValue(t, null);
            //if (item.GetType() == typeof(IEnumerable<T>))
            //    str += item.ToStringProperty();
            if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                // אם סוג התכונה הוא אוסף, הדפסת האיברים
                IEnumerable<T> collection = (IEnumerable<T>)item.GetValue(t);
                str += string.Join(", ", collection);
            }
            else
            {
                // אם לא, הדפסת הערך
                str += item.GetValue(t, null);
            }

        }
        return str;
    }

}
