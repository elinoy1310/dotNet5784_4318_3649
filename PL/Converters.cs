
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
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

public class ConvertTaskToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.TaskInEngineer? engTask = (BO.TaskInEngineer)value;
        if (engTask is null)
            return Visibility.Collapsed;
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class ConvertBooleanToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value)
            return Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertIntToVisibilityForEng : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((int)value!=0)
            return Visibility.Visible;
        return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ListItemSelectionConverter : IMultiValueConverter
{
    //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    IEnumerable<TaskInList>? depList=(IEnumerable<TaskInList>)value;
    //    foreach (TaskInList dep in depList)
    //        return dep.Select((BO.Task)parameter);
    //    return false;
    //}

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        // Create a new list to store the selection status of each item
        var selectionStatus = new List<bool>();
        if (values != null && values.Length == 2 )
        {
            // Iterate through each item in the list from the DependencyProperty
            foreach (var item in (IEnumerable<TaskInList>)values[0])
            {
                // Check if the item exists in the list from the non-DependencyProperty
                bool isSelected = item.Select((BO.Task)values[1]);

                // Add the selection status to the list
                selectionStatus.Add(isSelected);
            }

            //return selectionStatus;
        }
        return selectionStatus;
    }

    //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //{
    //    throw new NotImplementedException();
    //}

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}



public class ConvertOppositeBooleanToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(bool)value)
            return Visibility.Visible;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertStatusToBackground : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case "Unscheduled":
                return Brushes.Gray;
            case "Done":
                return Brushes.LightGreen;
            case "Scheduled":
                return Brushes.Yellow;
            case "OnTrack":
                return Brushes.LightSkyBlue;
            case "InJeopredy":
                return Brushes.LightPink;
            default:
                return Brushes.White;
        }

    }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

//public class ConvertBoolToBackground : IValueConverter
//{
//    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        return (bool)value ? Brushes.LightGray : Brushes.White;
      
//    }

//    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//    {
//        throw new NotImplementedException();
//    }
//}


public class ConvertStatusToForeground : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case "Unscheduled":
                return Brushes.Gray;
            case "Done":
                return Brushes.LightGreen;
            case "Scheduled":
                return Brushes.Yellow;
            case "OnTrack":
                return Brushes.LightSkyBlue;
            case "InJeopredy":
                return Brushes.LightPink;
            case "None":
                return Brushes.White;
            default:
                return Brushes.Black;
        }

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertTaskInlIstToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.TaskInList? curTask = (BO.TaskInList)value;
        return curTask is not null && curTask .Id!=0? curTask.ToString() : "Choose the dependencies that the new task is depend on them";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertTaskInEngineerToText : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.TaskInEngineer? engTask = (BO.TaskInEngineer)value;
        return engTask is not null ? engTask.ToString() : "you are not working on any task right now";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertIntToTextForDepWindow : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //1=list is not enabled= edit mode
        //else: add/update
        return (int)value == 1 ? "Edit the list of dependencies" : "Add / Update Dependencies for this task";
    }

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
            return Visibility.Visible;
        }
        else
        {
            return Visibility.Collapsed;
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
public class ConvertText1ToIsEnabled : IValueConverter
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
        if(value is not null)
         return (int)value == 0;
        return true;
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

public class ConvertText2ToIsEnabled : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 1;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}

public class ConvertByItemIdtoIsSelected : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        // if (value is TaskInList)
        // {
        BO.Task currentTask = s_bl.Task.Read((int)parameter);
        if (currentTask.Dependencies?.Count() == 0)
            return false;
        foreach (var dep in currentTask.Dependencies!)
            if (dep.Id == (int)value)
                return true;
        return false;

        //   }
    }
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
        if (lstEng.Count() == 0)
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





