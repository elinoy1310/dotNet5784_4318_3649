using System;
using System.Collections.Generic;
using System.Data;
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
using BO;

namespace PL
{

    /// <summary>
    /// Interaction logic for Gant.xaml
    /// </summary>
    public partial class Gant : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public IEnumerable<GanttTask>? ListGantTasks { get; set; }

        public DateTime StartDateColumn { get; set; }
        public DateTime CompleteDateColumn { get; set; }


        public Gant()
        {
       
            ListGantTasks = s_bl.CreateGantList();
            if(ListGantTasks is not null&&ListGantTasks!.Count()!=0)
            {

            StartDateColumn = ListGantTasks!.First().StartDate;
            CompleteDateColumn = ListGantTasks!.Max(task => task.CompleteDate);
            }
            else
            {
                StartDateColumn = s_bl.Clock;
                CompleteDateColumn = s_bl.Clock;
            }
            InitializeComponent();
        }



        private string dependenciesAsString(IEnumerable<int>? dependencies)
        {
            if (dependencies?.Count() == 0)
                return "";
            string str = "";
            foreach (var item in dependencies!)
            {

                str += $" {item},";
            }
            return str.Remove(str.Length-1);
        }

        private void GantGrid_Initialized(object sender, EventArgs e)
        {
            DataGrid? dg = sender as DataGrid;

            DataTable dt = new DataTable();

            if (dg != null)
            {
                dg.Columns.Add(new DataGridTextColumn() { Header = "Task Id", Binding = new Binding("[0]") });
                dt.Columns.Add("Task Id", typeof(int));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Task Name", Binding = new Binding("[1]") });
                dt.Columns.Add("Task Name", typeof(string));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Engineer Id", Binding = new Binding("[2]") });
                dt.Columns.Add("Engineer Id", typeof(string));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Engineer Name", Binding = new Binding("[3]") });
                dt.Columns.Add("Engineer Name", typeof(string));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Task dependencies", Binding = new Binding("[4]") });
                dt.Columns.Add("Task dependencies", typeof(string));

                int column = 5;
                for (DateTime date = StartDateColumn.Date; date <= CompleteDateColumn; date = date.AddDays(1))
                {
                    string strDate = $"{date.Day}/{date.Month}/{date.Year}";
                    dg.Columns.Add(new DataGridTextColumn() { Header = strDate, Binding = new Binding($"[{column}]") });
                    dt.Columns.Add(strDate, typeof(BO.Status));
                    column++;
                }

                if (ListGantTasks is not null)
                {
                    foreach (BO.GanttTask g in ListGantTasks)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = g.TaskId;
                        row[1] = g.TaskAlias;
                        row[2] = g.EngineerId;
                        row[3] = g.EngineerName;
                        row[4] = dependenciesAsString(g.DependentTasks);


                        int rows = 5;

                        for (DateTime date = StartDateColumn.Date; date <= CompleteDateColumn; date = date.AddDays(1))
                        {
                            // string strDate = $"{date.Day}/{date.Month}/{date.Year}";
                            if (date.Date < g.StartDate.Date || date.Date > g.CompleteDate.Date)
                                row[rows] = BO.Status.None;
                            else
                            {
                                row[rows] = g.Status;
                                if (s_bl.Task.checkInJeoprady(g.TaskId))
                                row[rows] = BO.Status.InJeopredy;
                            }
                            rows++;
                        }
                        dt.Rows.Add(row);
                    }

                    if (dt is not null)
                    {
                        dg.ItemsSource = dt.DefaultView;
                    }
                }

            }
        }
    }

}


