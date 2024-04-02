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

namespace PL
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public List<BO.Task> tasks { get; set; } = s_bl.UpdateManuallyList().ToList();
        // public BO.Task currentTask { get; set; }
       // string Time;

        public ScheduleWindow()
        {
            CurrentTask=tasks.First();
            InitializeComponent();
          
        }



        public TimeSpan ScheduleTime
        {
            get { return (TimeSpan)GetValue(ScheduleTimeProperty); }
            set { SetValue(ScheduleTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScheduleTimeProperty =
            DependencyProperty.Register("ScheduleTime", typeof(TimeSpan), typeof(ScheduleWindow), new PropertyMetadata(null));


        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for currentTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(ScheduleWindow), new PropertyMetadata(null));


        public DateTime ScheduleDate
        {
            get { return (DateTime)GetValue(ScheduleDateProperty); }
            set { SetValue(ScheduleDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScheduleDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScheduleDateProperty =
            DependencyProperty.Register("ScheduleDate", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(s_bl.Clock));

        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if(!TimeSpan.TryParse(Time,out TimeSpan sceduleTime))
                //{
                //    MessageBox.Show("Invalid time format, please enter time in HH:MM:SS format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                ScheduleDate= ScheduleDate.AddHours(ScheduleTime.Hours - ScheduleDate.Hour);
                ScheduleDate = ScheduleDate.AddMinutes(ScheduleTime.Minutes - ScheduleDate.Minute);
                ScheduleDate= ScheduleDate.AddSeconds(ScheduleTime.Seconds - ScheduleDate.Second);
                MessageBoxResult mbResultEx = MessageBox.Show("Are you sure you want to set the schedule date for this task to "+ScheduleDate+" ? you can't go back", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mbResultEx == MessageBoxResult.Yes)
                {
                    
                    s_bl.CreateSchedule(ScheduleDate, BO.CreateScheduleOption.Manually, CurrentTask.Id);
                    MessageBox.Show("The task has been updated successfully!");
                    tasks.RemoveAt(0);
                    if (tasks.Count() == 0)
                    {
                        MessageBox.Show("All the tasks are updated, The Schedule was created successfully!");
                        this.Close();
                    }
                    else
                    {
                 //       tasks=(from task in tasks select s_bl.Task.Read(task.Id)).ToList();                       
                        CurrentTask = tasks.First();
                    }
                        
                } 

                
            }
            catch (BO.BlCannotBeUpdatedException ex)
            {
                MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (BO.BlCanNotBeNullException ex)
            {
                MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBoxResult mbResultEx = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                MessageBoxResult mbResultEx = MessageBox.Show("UnKnown error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
