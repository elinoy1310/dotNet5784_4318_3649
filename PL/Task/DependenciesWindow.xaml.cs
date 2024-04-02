//using BO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL.Task
//{
//    /// <summary>
//    /// Interaction logic for DependenciesWindow.xaml
//    /// </summary>
//    public partial class DependenciesWindow : Window
//    {
//        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
//        public  BO.Task Task { get; set; }
//        //public List<bool> SelectItems { get; set; }
//        public DependenciesWindow(int idTask)
//        {
//            InitializeComponent();

//          //  CurrentDependencies = Task.Dependencies;

//            //Dependencies = s_bl.Task.ReadAll(task => task.Id != idTask);
//            Task = s_bl.Task.Read(idTask);
//            Dependencies=s_bl.Task.ReadAll(task=>task.Id!=idTask);
//            //SelectItems = new List<bool>();
//            //foreach (var item in Dependencies)
//            //{
//            //    if(item.Select(Task))
//            //        SelectItems.Add(true);
//            //    else
//            //        SelectItems.Add(false);
//            //}




//            //ListBox lb=new ListBox();
//            //foreach(var dep in task.Dependencies)


//        }


//        public IEnumerable<TaskInList>? CurrentDependencies
//        {
//            get { return (IEnumerable<TaskInList>)GetValue(CurrentDependenciesProperty); }
//            set { SetValue(CurrentDependenciesProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for CurrentDependencies.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty CurrentDependenciesProperty =
//            DependencyProperty.Register("CurrentDependencies", typeof(IEnumerable<TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));


//        public bool IsSelected(int id,int idTask)
//        {
//            //BO.Task task = s_bl.Task.Read(idTask);
//            //foreach (var dep in task.Dependencies)
//            ////    if (dep.Id == id)
//                   return false;
//            //return true;
//        }

//        public IEnumerable<TaskInList> Dependencies
//        {
//            get { return (IEnumerable<TaskInList>)GetValue(DependenciesProperty); }
//            set { SetValue(DependenciesProperty, value); }
//        }

//        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
//        public static readonly DependencyProperty DependenciesProperty =
//            DependencyProperty.Register("Dependencies", typeof(IEnumerable<TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));

//        private void ListBox_Initialized(object sender, EventArgs e)
//        {
//            //foreach(var item in (sender as ListBox).Items)
//            //{
//            //    if (item is TaskInList)
//            //    {
//            //        TaskInList taskInList = (TaskInList)item;
//            //        if(taskInList.Select(Task))

//            //    }
//            //}
//        }

//        private void ListBoxItem_Initialized(object sender, EventArgs e)
//        {



//        }



//        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {

//        }
//    }
//}

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
        //public bool isSelected { get; set; }
        public TaskWindow Tw { get; set; }
        //public DependenciesWindow(int idTask,TaskWindow tw)
        //{
        //    InitializeComponent();
        //    Dependencies = s_bl.Task.ReadAll(task => task.Id != idTask);
        //    currentTask = s_bl.Task.Read(idTask);
        //    Tw= tw;
        //    //ListBox lb=new ListBox();
        //    //foreach(var dep in task.Dependencies)


        //}
        public DependenciesWindow(BO.Task auTask, TaskWindow tw)
        {
            InitializeComponent();
            Dependencies = s_bl.Task.ReadAll(task => task.Id != auTask.Id).OrderBy(task=>task.Id);
            currentTask = auTask;
            Tw = tw;
            Add_UpdateDep=new List<TaskInList>();
            //ListBox lb=new ListBox();
            //foreach(var dep in task.Dependencies)
        }


        public List<TaskInList> Add_UpdateDep { get; set; }
        //{
        //    get { return (IEnumerable<TaskInList>)GetValue(Add_UpdateDepProperty); }
        //    set { SetValue(Add_UpdateDepProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Add_UpdateDep.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty Add_UpdateDepProperty =
        //    DependencyProperty.Register("Add_UpdateDep", typeof(IEnumerable<TaskInList>), typeof(DependenciesWindow), new PropertyMetadata(null));



        //public bool IsSelected(int id, int idTask)
        //{
        //    BO.Task task = s_bl.Task.Read(idTask);
        //    foreach (var dep in task.Dependencies)
        //        if (dep.Id == id)
        //            return false;
        //    return true;
        //}
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
            //if (Add_UpdateDep == currentTask.Dependencies)//אם נשאר באותו מצב
            //    return;
           currentTask.Dependencies = Add_UpdateDep;//+updated dep list
            //new TaskWindow(currentTask).Show();//
            this.Close();
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
           //     Add_UpdateDep = ((sender as ListBox)!.SelectedItems as IList<TaskInList>);
            
            //else if ((sender as ListBox) is not null && (sender as ListBox)!.SelectedItems.Count == 0) ;
            //Add_UpdateDep = null;
          
        }
    }
}

