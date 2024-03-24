﻿using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        public BO.Status Status { get; set; } = BO.Status.Unscheduled;
        public Func<BO.Task,bool>? Filter{ get; set; }
        public int IdUser { get; set; }

        public TaskListWindow()
        {
            InitializeComponent();
            IdUser= 0;
        }

        public TaskListWindow(BO.Engineer eng)
        {
            InitializeComponent();
            Filter = item => item.Engineer is null&&s_bl.Task.PreviousTaskDone(item.Id)&&item.Status!=BO.Status.Done;
            IdUser=eng.Id;
        }



        public DateTime FilterStartDate
        {
            get { return (DateTime)GetValue(FilterStartDateProperty); }
            set { SetValue(FilterStartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilterStartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilterStartDateProperty =
            DependencyProperty.Register("FilterStartDate", typeof(DateTime), typeof(TaskListWindow), new PropertyMetadata(null));



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




        public BO.Task SelectedTask
        {
            get { return (BO.Task)GetValue(SelectedTaskProperty); }
            set { SetValue(SelectedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTaskProperty =
            DependencyProperty.Register("SelectedTask", typeof(BO.Task), typeof(TaskListWindow), new PropertyMetadata(null));


        private void CbFilterByLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TaskList = Complexity == BO.EngineerExperience.None ? s_bl.Task.ReadAll(Filter) : s_bl.Task.ReadAll(task => (task.Complexity == Complexity && Filter == null ? true : Filter!(task)));
        }
        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {

            new TaskWindow().ShowDialog();
            TaskList = s_bl.Task.ReadAll();
        }

        private void lvSelectTaskToUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (IdUser !=0)
            {
                BO.Task updateTask = s_bl.Task.Read(task!.Id);
                BO.EngineerInTask engInTask= new BO.EngineerInTask() { Id = (int)IdUser };
                updateTask!.Engineer = engInTask;
                s_bl.Task.Update(updateTask);            
                this.Close();
                new EngineerViewWindow(IdUser).Show();
            }
            else
            {
                new TaskWindow(task?.Id ?? 0).ShowDialog();
                TaskList = s_bl.Task.ReadAll(Filter);
            }

        }

        private void wLoadTheUpdatedTasksList_Loaded(object sender, RoutedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAll(Filter);
        }

        private void CbFilterByStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = Status == BO.Status.Unscheduled ? s_bl.Task.ReadAll(Filter) : s_bl.Task.ReadAll(task => task.Status == Status && Filter == null ? true : Filter!(task));
        }

        private void btnFilterByStartDate_Click(object sender, RoutedEventArgs e)
        {
            TaskList = s_bl.Task.ReadAll(task => task.StartDate == FilterStartDate && Filter == null ? true : Filter!(task));
        }

        private void btnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTask is not null)
                s_bl.Task.Delete(SelectedTask.Id);
            MessageBoxResult successMsg = MessageBox.Show("The Task deleted successfully!");
            TaskList = s_bl.Task.ReadAll();
        }

        private void selectTask(object sender, SelectionChangedEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task is not null)
                SelectedTask = s_bl.Task.Read(task.Id);
        }

    }
}
