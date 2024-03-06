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

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Complexity { get; set; } = BO.EngineerExperience.None;
        public TaskListWindow()
        {
            InitializeComponent();
        }

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskInListProperty); }
            set { SetValue(TaskInListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TaskList.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TaskListProperty =
        //    DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(0));

        public static readonly DependencyProperty TaskInListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));


        private void CbFilterByLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = Complexity == BO.EngineerExperience.None ? s_bl.Task.ReadAll() : s_bl.Task.ReadAll(task => task.Complexity == Complexity);
        }
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            new TaskWindow().ShowDialog();
            TaskList = s_bl.Task.ReadAll();
        }

        private void lvSelectTaskToUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            new TaskWindow(task?.Id ?? 0).ShowDialog();
            TaskList = s_bl.Task.ReadAll();
        }

        private void wLoadTheUpdatedTasksList_Loaded(object sender, RoutedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAll();
        }
    }
}
