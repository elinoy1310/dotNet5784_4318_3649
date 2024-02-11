using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace BO;

public static class Tools
{
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
                    // str += value;
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
//public static string ToStringProperty<T>(this T t) where T:notnull
//{
//    var str = new StringBuilder();
//    var properties = t!.GetType().GetProperties();
//    var newLine = Environment.NewLine;

//    foreach (var property in properties)
//    {
//        var value = property.GetValue(t);
//        if (value is IEnumerable<T>)
//        {
//            string strValue = $"{property.Name}:{newLine} {string.Join(newLine, value)}";
//            str.AppendLine(strValue);
//        }
//        str.AppendLine($"{property.Name}: {value}");
//    }
//    return str.ToString();
//string str = "";
//foreach (PropertyInfo item in t!.GetType().GetProperties())
//{
//    str += "\n" + item.Name + ": " + item.GetValue(t, null);

//    if (item.GetType() == typeof(System.Collections.Generic.List`1[BO.TaskInList]))
//    {
//        str += item.ToStringProperty();
//    }
//    //    //        //foreach (PropertyInfo item2 in item!.GetType().GetProperties())
//    //        //{
//    //        //        str += "\n" + item2.Name + ": ";
//    //        //    foreach (PropertyInfo item3 in item2.GetType().GetProperties())
//    //        //        str += item2.GetValue(t, null);
//    //        //}
//    //            //    str += item.ToStringProperty();
//    //            //if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
//    //            //{
//    //            //    // אם סוג התכונה הוא אוסף, הדפסת האיברים
//    //            //    IEnumerable<T> collection = (IEnumerable<T>)item.GetValue(t);
//    //            //    str += string.Join(", ", collection);
//    //            //}
//    //            //else
//    //            //{
//    //            //    // אם לא, הדפסת הערך
//    //            //    str += item.GetValue(t, null);
//    //            //}

//    }
// return str;
