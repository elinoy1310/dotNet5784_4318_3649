using BlApi;
using BlImplementation;
using BO;
using PL.Engineer;
using PL.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool ShowPreviousPasswordIsClicked { get; set; }
        public int idUser {  get; set; }
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public ManagerWindow(int id)
        {
            InitializeComponent();
            startProject = s_bl.ProjectStartDate;
            ChangePasswordIsClicked = false;
            ShowPreviousPasswordIsClicked = false;
            idUser= id;
        }



        public DateTime? startProject
        {
            get { return (DateTime)GetValue(startProjectProperty); }
            set { SetValue(startProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for startProject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty startProjectProperty =
            DependencyProperty.Register("startProject", typeof(DateTime?), typeof(ManagerWindow), new PropertyMetadata(null));

        public bool ChangePasswordIsClicked
        {
            get { return (bool)GetValue(ChangePasswordIsClickedProperty); }
            set { SetValue(ChangePasswordIsClickedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangePasswordIsClickedProperty =
            DependencyProperty.Register("ChangePasswordIsClicked", typeof(bool), typeof(ManagerWindow), new PropertyMetadata(false));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ManagerWindow), new PropertyMetadata(""));

      

       
        private void ShowPassword_Click(object sender, MouseButtonEventArgs e)
        {
            if (!ShowPreviousPasswordIsClicked)
            {
                ShowPreviousPasswordIsClicked = true;
                Password = s_bl.User.Read(idUser).passWord!;
            }
            else
            {
                ShowPreviousPasswordIsClicked = false;
                Password = "";
            }

        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordIsClicked = true;
        }

        private void updatePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Password == s_bl.User.Read(idUser).passWord)
            {
                MessageBoxResult mbResultSame = MessageBox.Show("This is the same password as before", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (mbResultSame == MessageBoxResult.OK)
                    btnCancel_Click(sender, e);
            }
            else
            {
                MessageBoxResult mbResult = MessageBox.Show($"Are you sure you want to change your password to {Password}?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbResult == MessageBoxResult.Yes)
                {
                    s_bl.User.Update(new BO.User() { UserId = idUser, UserType = BO.UserType.Engineer, passWord = Password });
                    ChangePasswordIsClicked = false;
                    Password = "";
                }

            }

        }

        /// <summary>
        /// Handles the click event of the "Engineers List" button.
        /// Opens the EngineerListWindow to display the list of engineers.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void btnEnginnersList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();
        }


        /// <summary>
        /// Handles the click event of the "Initialization" button.
        /// Displays a confirmation message box to initialize all the data if the user chooses "Yes".
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to initialize all the data?", "Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(mbResult == MessageBoxResult.Yes) 
            {
               s_bl.ResetDB();
               s_bl.InitializeDB();
            }
        }


        /// <summary>
        /// Handles the click event of the "Reset" button.
        /// Displays a confirmation message box to clear all the data if the user chooses "Yes".
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to clear all the data?", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                s_bl.ResetDB();
            }
        }

        private void btnTaskList_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();
        }

        private void btnScedule_click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.ProjectStartDate = startProject;


                MessageBoxResult mbResult = MessageBox.Show("Are you sure you want to create the schedule automatically?", "Create Schedule Option", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (mbResult == MessageBoxResult.Yes)
                {
                    try
                    {
                        s_bl.CreateSchedule(s_bl.Clock,BO.CreateScheduleOption.Automatically);
                        MessageBox.Show("The scheduled has created successfuly!");
                    }
                    catch (BO.BlWrongDataException ex)
                    {
                        MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    catch (BO.BlDoesNotExistException ex)
                    {
                        MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch
                    {
                        MessageBoxResult mbResultEx = MessageBox.Show("UnKnown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (mbResult == MessageBoxResult.No)
                {
                    new ScheduleWindow().ShowDialog();
                }
            }
            catch( BO.BlCannotBeUpdatedException ex)

            {
                MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGant_Click(object sender, RoutedEventArgs e)
        {
            new Gant().ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordIsClicked = false;
            Password = "";
        }
        
          
        



       


       

       

       
        
    }
}



//Visibility="{Binding IsChecked ,ElementName CreateSchedule, Converter={StaticResource ConvertBooleanToVisibility}}"