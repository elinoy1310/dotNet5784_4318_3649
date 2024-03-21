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
using PL.Engineer;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        public string State { get; init; }

      

        public TaskWindow(int idTask = 0)
        {
            InitializeComponent();
            if (idTask == 0)
            {
                add_updateTask = new BO.Task();
                State = "Add";
            }
            else
            {
                State = "Update";
                try
                {
                    add_updateTask = s_bl.Task.Read(idTask);

                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new TaskListWindow().Show();
                    }
                }

            }



        }

        public BO.Task add_updateTask
        {
            get { return (BO.Task)GetValue(add_updateTaskProperty); }
            set { SetValue(add_updateTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for add_updateTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty add_updateTaskProperty =
            DependencyProperty.Register("add_updateTask", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(new BO.Task()));
       
        
        private void AddOrUpdateClick(object sender, RoutedEventArgs e)
        {
            if (State == "Add")
            {
                try
                {
                    s_bl.Task.Create(add_updateTask);
                    MessageBoxResult successMsg = MessageBox.Show("The Task created successfully!");
                }
                //catch (BO.BlAlreadyExistException ex)
                //{
                //    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //}
                catch (BO.BlWrongDataException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
            else // State is "Update"
            {
                try
                {
                    s_bl.Task.Update(add_updateTask);
                    MessageBoxResult successMsg = MessageBox.Show("The engineer updated successfully!");
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                //catch (BO.BlWrongDataException ex)
                //{
                //    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //}
                //catch (BO.BlCannotBeUpdatedException ex)
                //{
                //    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                //}

            }

            this.Close();
            s_bl.Task.ReadAll();
        }

        private void BtnDependencies_Click(object sender, RoutedEventArgs e)
        {
            new DependenciesWindow(add_updateTask.Id).ShowDialog();
        }
    }
}
