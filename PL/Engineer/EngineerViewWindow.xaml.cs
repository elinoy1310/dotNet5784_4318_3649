

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
using System.Windows.Shapes;
//using PL.Task;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public bool ShowPreviousPasswordIsClicked { get; set; }
        public EngineerViewWindow(int engineerId)
        { 
            ChangePasswordIsClicked = false;
            ShowPreviousPasswordIsClicked = false;
            InitializeComponent();
            try
            {
                EngineerDetails = s_bl.Engineer.Read(engineerId);

            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (mbResult == MessageBoxResult.OK)
                {
                    new MainWindow().Show();
                }
            }
           
        }



        public bool ChangePasswordIsClicked
        {
            get { return (bool)GetValue(ChangePasswordIsClickedProperty); }
            set { SetValue(ChangePasswordIsClickedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangePasswordIsClickedProperty =
            DependencyProperty.Register("ChangePasswordIsClicked", typeof(bool), typeof(EngineerViewWindow), new PropertyMetadata(false));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(EngineerViewWindow), new PropertyMetadata(""));



        public BO.Engineer EngineerDetails
        {
            get { return (BO.Engineer)GetValue(EngineerDetailsProperty); }
            set { SetValue(EngineerDetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EngineerDetails.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerDetailsProperty =
            DependencyProperty.Register(
                "EngineerDetails", typeof(BO.Engineer), typeof(EngineerViewWindow), new PropertyMetadata(new BO.Engineer()));

        private void btnCompleteTask_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show(
                "Have you finished the task completely?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                BO.Engineer updateEng = EngineerDetails;
                updateEng.Task = null;
                s_bl.Engineer.Update(EngineerDetails);
                EngineerDetails = s_bl.Engineer.Read(updateEng.Id);
            }
        }

        private void btnChooseNewTask_click(object sender, RoutedEventArgs e)
        {
            if (EngineerDetails.Task is not null)
            {
                MessageBoxResult mbResult = MessageBox.Show(
                    "This action will End your current Mission, are you sure you want to choose new task?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (mbResult == MessageBoxResult.No)
                {
                    return;
                }
                btnCompleteTask_click(sender, e);
            }
            new TaskListWindow(EngineerDetails).Show();
            this.Close();
            // EngineerDetails = s_bl.Engineer.Read(EngineerDetails.Id);
        }

        private void ShowPassword_Click(object sender, MouseButtonEventArgs e)
        {
            if (ShowPreviousPasswordIsClicked)
            {
                ShowPreviousPasswordIsClicked = false;
                Password = s_bl.User.Read(EngineerDetails.Id).passWord!;
            }
            else
            {
                ShowPreviousPasswordIsClicked = true;
                Password = "";
            }

        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
         {
            ChangePasswordIsClicked = true;
        }

        private void updatePassword_Click(object sender, RoutedEventArgs e)
        {
            if(Password==s_bl.User.Read(EngineerDetails.Id).passWord)
            {
                MessageBoxResult mbResultSame = MessageBox.Show("This is the same password as before", "Information", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if(mbResultSame==MessageBoxResult.OK)
                    btnCancel_Click(sender, e);
            }
               
            MessageBoxResult mbResult = MessageBox.Show($"Are you sure you want to change your password to {Password}?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                s_bl.User.Update(new BO.User() { UserId = EngineerDetails.Id, UserType = BO.UserType.Engineer, passWord = Password });
                ChangePasswordIsClicked = false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordIsClicked = false;
        }
    }
}

