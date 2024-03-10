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
        public IEnumerable<BO.Task> tasks { get; set; } = s_bl.UpdateManuallyList();
        public BO.Task currentTask { get; set; }

        public ScheduleWindow()
        {
            currentTask=tasks.First();
            InitializeComponent();
        }

        public DateTime ScheduleDate
        {
            get { return (DateTime)GetValue(ScheduleDateProperty); }
            set { SetValue(ScheduleDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScheduleDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScheduleDateProperty =
            DependencyProperty.Register("ScheduleDate", typeof(DateTime), typeof(ScheduleWindow), new PropertyMetadata(null));

        private void btnUpdateDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.CreateSchedule(ScheduleDate, BO.CreateScheduleOption.Manually, currentTask.Id);
                MessageBox.Show("The task has been updated successfully!");
                tasks.ToList().RemoveAt(0);
                if (tasks.Count() == 0)
                    MessageBox.Show("All the tasks are updated, The Schedule was created successfully!");
                else
                currentTask = tasks.First();
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
