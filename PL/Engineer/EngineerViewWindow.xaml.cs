

using PL.Task;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerViewWindow.xaml
    /// </summary>
    public partial class EngineerViewWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerViewWindow(int engineerId)
        {
            InitializeComponent();
            try
            {
                EngineerDetails = s_bl.Engineer.Read(engineerId);

            }
            catch (BO.BlDoesNotExistException ex)
            {
                MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (mbResult == MessageBoxResult.OK)
                {
                    new MainWindow().Show();
                }
            }
        }


        public BO.Engineer EngineerDetails
        {
            get { return (BO.Engineer)GetValue(EngineerDetailsProperty); }
            set { SetValue(EngineerDetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EngineerDetails.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerDetailsProperty =
            DependencyProperty.Register("EngineerDetails", typeof(BO.Engineer), typeof(EngineerViewWindow), new PropertyMetadata(new BO.Engineer()));

        private void btnCompleteTask_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbResult = MessageBox.Show("Have you finished the task completely?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (mbResult == MessageBoxResult.Yes)
            {
                BO.Engineer updateEng = EngineerDetails;
                updateEng.Task = null;
                s_bl.Engineer.Update(EngineerDetails);
                EngineerDetails = s_bl.Engineer.Read(updateEng.Id);
            }
        }

        private void btnChooseNewTask_click(object sender, RoutedEventArgs e)
        {
            if(EngineerDetails.Task is not null)
            {
                MessageBoxResult mbResult = MessageBox.Show("This action will End your current Mission, are you sure you want to choose new task?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (mbResult == MessageBoxResult.No)
                {
                    return;
                }
                btnCompleteTask_click(sender, e);
            }
           new TaskListWindow( EngineerDetails).ShowDialog();
        }
    }
}

