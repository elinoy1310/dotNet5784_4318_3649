using BlImplementation;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace BO;

public static class Tools
{
    /// <summary>
    /// Converts the properties of the specified object to a string representation, optionally using the specified suffix.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <param name="t">The object whose properties to convert to a string.</param>
    /// <param name="suffix">The optional suffix to add to each property in the string.</param>
    /// <returns>A string representation of the object's properties.</returns>
    public static string ToStringProperty<T>(this T t, string suffix = "")
    {
        string str = "";
        foreach (PropertyInfo prop in t!.GetType().GetProperties())
        {

            var value = prop.GetValue(t, null);

            if (value is IEnumerable)
            {
                str += "\n" + suffix + prop.Name + ": ";
                foreach (var item in (IEnumerable)value)
                {
                    str += item.ToStringProperty(" ");

                }
                if (value is string)
                    str += value;


            }
            else
                str += "\n" + suffix + prop.Name + ": " + value;


        }
        return str;
    }

}
