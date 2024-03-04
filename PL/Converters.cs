
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using BO;

namespace PL;


/// <summary>
/// Converts an integer ID value to corresponding content for UI purposes.
/// </summary>
class ConvertIdToContent : IValueConverter
{
    /// <summary>
    /// Converts the integer ID value to content based on whether it's 0 or not.
    /// </summary>
    /// <param name="value">The integer ID value.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Returns "Add" if the ID is 0, otherwise returns "Update".</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    /// <summary>
    /// Converts back a value to an integer ID. This operation is not supported and will throw a NotImplementedException.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Converts text or integer value to visibility for UI purposes.
/// </summary>
public class ConvertTextToVisibility : IValueConverter
{
    /// <summary>
    /// Converts the input value to visibility based on whether it represents "Add" or not.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Returns Visibility.Hidden if the value represents "Add" or 0, otherwise returns Visibility.Visible.</returns>
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

    /// <summary>
    /// Converts back a value to its original representation. This operation is not supported and will throw a NotImplementedException.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}



/// <summary>
/// Converts text or integer value to a boolean indicating whether an element should be enabled or disabled for UI purposes.
/// </summary>
public class ConvertTextToIsEnabled : IValueConverter
{
    /// <summary>
    /// Converts the input value to a boolean indicating whether the element should be enabled or disabled.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Returns true if the value is 0, indicating "Add"; otherwise, returns false, indicating "Update".</returns>
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

    /// <summary>
    /// Converts back a value to its original representation. This operation is not supported and will throw a NotImplementedException.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


/// <summary>
/// Converts a list of engineers to visibility for UI purposes.
/// </summary>
public class ConvertListToVisibility : IValueConverter
{

    /// <summary>
    /// Converts the input list of engineers to visibility based on whether it is empty or not.
    /// </summary>
    /// <param name="value">The input list of engineers.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Returns Visibility.Visible if the list is empty, otherwise returns Visibility.Hidden.</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        IEnumerable<BO.Engineer> lstEng = (IEnumerable<BO.Engineer>)value;
        if (lstEng.Count()==0)
        {
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Hidden;
        }
    }

    /// <summary>
    /// Converts back a value to its original representation. This operation is not supported and will throw a NotImplementedException.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">An optional parameter.</param>
    /// <param name="culture">The culture to use in the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


