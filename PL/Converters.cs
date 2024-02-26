
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BO;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertTextToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //string strValue = (string)value;
        if (/*strValue=="Add"*/(int)value == 0)
        {
            return Visibility.Hidden; 
        }
        else
        {
            return Visibility.Visible;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertTextToIsEnabled : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0;
        //string strValue = (string)value;
        //if (strValue == "Add")
        //{
        //    return Visibility.Hidden;
        //}
        //else
        //{
        //    return Visibility.Visible;
        //}
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


