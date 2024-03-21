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
        public bool isSelected { get; set; }
        public DependenciesWindow(int idTask)
        {
            InitializeComponent();
            Dependencies= s_bl.Task.ReadAll(task=>task.Id!=idTask);
            BO.Task task=s_bl.Task.Read(idTask);
            //ListBox lb=new ListBox();
            //foreach(var dep in task.Dependencies)
                

        }

        public bool IsSelected(int id,int idTask)
        {
            BO.Task task = s_bl.Task.Read(idTask);
            foreach (var dep in task.Dependencies)
                if (dep.Id == id)
                    return false;
            return true;
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
            ListBoxItem? t = sender as ListBoxItem/*)?. as BO.TaskInList;*/;
         //   t as TaskInList


        }
    }
}
