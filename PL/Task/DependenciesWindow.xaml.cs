

using BO;
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
    /// Interaction logic for DependenciesWindow.xaml
    /// </summary>
    public partial class DependenciesWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Task currentTask { get; set; }
        public TaskInList DisplayTask { get; set; }
        //public bool isSelected { get; set; }
        public TaskWindow Tw { get; set; }
    
        public DependenciesWindow(BO.Task auTask, TaskWindow tw)
        {  
            DisplayTask=new TaskInList() { Id = auTask.Id, Alias=auTask.Alias,Description=auTask.Description,Status=auTask.Status };
            InitializeComponent();
            Dependencies = s_bl.Task.ReadAll(task => task.Id != auTask.Id).OrderBy(task=>task.Id);
            currentTask = auTask;
         
            Tw = tw;
            Add_UpdateDep=new List<TaskInList>();
        }



        public int EditIsClicked
        { 
            get { return (int)GetValue(EditIsClickedProperty); }
            set { SetValue(EditIsClickedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EditIsClicked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditIsClickedProperty =
            DependencyProperty.Register("EditIsClicked", typeof(int), typeof(DependenciesWindow), new PropertyMetadata(1));



        public List<TaskInList> Add_UpdateDep { get; set; }
       
        private void ListBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            TaskInList? depTask;
            if (sender as ListBoxItem is not null)
            {

                depTask = (sender as ListBoxItem)!.Content as TaskInList;
                if (depTask is not null)
                {
                    (sender as ListBoxItem)!.IsSelected = true;
                }

            }
        }
        public IEnumerable<TaskInList> Dependencies
        {
            get { return (IEnumerable<TaskInList>)GetValue(DependenciesProperty); }
            set { SetValue(DependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DependenciesProperty =
            DependencyProperty.Register("Dependencies", typeof(IEnumerable<TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));

        private void ListBox_Initialized(object sender, EventArgs e)
        {

        }

        private void ListBoxItem_Initialized(object sender, EventArgs e)
        {
            //ListBoxItem? t = sender as ListBoxItem/*)?. as BO.TaskInList;*/;
            ////   t as TaskInList


        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
      
            if((sender as ListBox)is not null && currentTask.Dependencies is not null)
            {
                for (int i = 0; i < (sender as ListBox)!.Items.Count; i++)
                {
                    TaskInList? depTask = (sender as ListBox)!.Items[i] as TaskInList;
                    if (depTask is not null)
                    {
                        if (currentTask.Dependencies!.FirstOrDefault(x => x.Id == depTask.Id) is not null)//item נמצא ברשימה של התלויות של task
                        {
                            (sender as ListBox)!.SelectedItems.Add((sender as ListBox)!.Items[i]);
                        }
                    }
                }
            }
            
            
            
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (EditIsClicked == 1)
                EditIsClicked = 0;
            else
            {

            //if (Add_UpdateDep == currentTask.Dependencies)//אם נשאר באותו מצב
            //    return;
           currentTask.Dependencies = Add_UpdateDep;//+updated dep list
            //new TaskWindow(currentTask).Show();//
            this.Close();
            }
        }

        private void lb_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((sender as ListBox) is not null)
            {
                Add_UpdateDep.Clear();
                foreach (var t in ((sender as ListBox)!.SelectedItems))
                {
                    Add_UpdateDep.Add((t as TaskInList)!);

                }

            }
          
        }
    }
}

