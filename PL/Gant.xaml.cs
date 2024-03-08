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
        public IEnumerable<GantTask>? ListGantTasks { get; set; }

        public DateTime StartDateColumn { get; set; }
        public DateTime CompleteDateColumn { get; set; }


        public Gant()
        {
            var lst = from item in s_bl.Task.ReadAll()
                      let task = s_bl.Task.Read(item.Id)
                      select new GantTask
                      {
                          TaskId = task.Id,
                          TaskAlias = task.Alias,
                          EngineerId = task.Engineer?.Id,
                          EngineerName = task.Engineer?.Name,
                          StartDate = calculateStartDate(task.StartDate, task.ScheduledDate),
                          CompleteDate = calculateCompleteDate(task.ForecastDate, task.CompleteDate),
                          DependentTasks = lstDependentId(task.Dependencies),
                          Status = task.Status
                      };
            ListGantTasks = lst.OrderBy(task => task.StartDate);
            StartDateColumn = ListGantTasks.First().StartDate;
            CompleteDateColumn=ListGantTasks.Max(task => task.CompleteDate);
            InitializeComponent();
        }

        private IEnumerable<int> lstDependentId(IEnumerable<TaskInList>? dependencies)
        {
            return from dep in dependencies
                   select dep.Id;
        }

        private DateTime calculateStartDate(DateTime? startDate, DateTime? scheduledDate)
        {
            if (startDate == null)
                return scheduledDate ?? s_bl.Clock;
            else
                return startDate ?? s_bl.Clock;
        }

        private DateTime calculateCompleteDate(DateTime? forecastDate, DateTime? completeDate)
        {
            if (completeDate == null)
                return forecastDate ?? s_bl.Clock;
            else
                return completeDate ?? s_bl.Clock;
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
                dt.Columns.Add("Engineer Id", typeof(int));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Engineer Name", Binding = new Binding("[3]") });
                dt.Columns.Add("Engineer Name", typeof(string));
                dg.Columns.Add(new DataGridTextColumn() { Header = "Task dependencies", Binding = new Binding("[4]") });
                dt.Columns.Add("Task dependencies", typeof(int/*IEnumerable<int>*/));
                int column = 5;
                for(DateTime date=StartDateColumn.Date; date<=CompleteDateColumn;date= date.AddDays(1))
                {
                    string strDate = $"{date.Day}/{date.Month}/{date.Year}";
                    dg.Columns.Add(new DataGridTextColumn() { Header = strDate, Binding = new Binding($"[{column}]") });
                    dt.Columns.Add(strDate, typeof(BO.Status));
                    column++;
                }

                if (ListGantTasks is not null)
                {
                    foreach (GantTask g in ListGantTasks)
                    {
                        DataRow row = dt.NewRow();
                        row[0] = g.TaskId;
                        row[1] = g.TaskAlias;
                        row[2] = 0;//g.EngineerId;
                        row[3] = g.EngineerName;
                        row[4] = 0;//g.DependentTasks;


                        int rows = 5;

                        for (DateTime date = StartDateColumn.Date; date <= CompleteDateColumn; date = date.AddDays(1))
                        {
                            // string strDate = $"{date.Day}/{date.Month}/{date.Year}";
                            if (date < g.StartDate || date > g.CompleteDate)
                                row[rows] = BO.Status.None;
                            else
                            {
                                row[rows]= g.Status;
                            }
                            rows++;
                        }
                       dt.Rows.Add(row);
                    }

                    if(dt is not null)
                    {
                        dg.ItemsSource = dt.DefaultView;
                    }
                }
                
            }
        }
    }

}


