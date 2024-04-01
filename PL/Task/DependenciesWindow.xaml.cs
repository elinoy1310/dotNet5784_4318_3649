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
        public  BO.Task Task { get; set; }
        public List<bool> SelectItems { get; set; }
        public DependenciesWindow(int idTask)
        {
            InitializeComponent();
           
          //  CurrentDependencies = Task.Dependencies;
            Dependencies = s_bl.Task.ReadAll(task => task.Id != idTask);
            Task = s_bl.Task.Read(idTask);
            SelectItems = new List<bool>();
            foreach (var item in Dependencies)
            {
                if(item.Select(Task))
                    SelectItems.Add(true);
                else
                    SelectItems.Add(false);
            }
         
            
            
  
            //ListBox lb=new ListBox();
            //foreach(var dep in task.Dependencies)
                

        }


        public IEnumerable<TaskInList>? CurrentDependencies
        {
            get { return (IEnumerable<TaskInList>)GetValue(CurrentDependenciesProperty); }
            set { SetValue(CurrentDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDependencies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDependenciesProperty =
            DependencyProperty.Register("CurrentDependencies", typeof(IEnumerable<TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));


        public bool IsSelected(int id,int idTask)
        {
            //BO.Task task = s_bl.Task.Read(idTask);
            //foreach (var dep in task.Dependencies)
            ////    if (dep.Id == id)
                   return false;
            //return true;
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
            //foreach(var item in (sender as ListBox).Items)
            //{
            //    if (item is TaskInList)
            //    {
            //        TaskInList taskInList = (TaskInList)item;
            //        if(taskInList.Select(Task))
                        
            //    }
            //}
        }

        private void ListBoxItem_Initialized(object sender, EventArgs e)
        {
        


        }

        private void ListBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            //TaskInList? depTask=(sender as ListBoxItem).;
        }
    }
}
