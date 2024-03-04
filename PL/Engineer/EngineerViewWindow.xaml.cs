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
            catch (BO.Engineer.BlDoesNotExistException ex)
            {
                MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if (mbResult == MessageBoxResult.OK)
                {
                    new MainWindow().Show();
                }
            }
        }


        public BO.Engineer.Engineer EngineerDetails
        {
            get { return (BO.Engineer.Engineer)GetValue(EngineerDetailsProperty); }
            set { SetValue(EngineerDetailsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EngineerDetails.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerDetailsProperty =
            DependencyProperty.Register("EngineerDetails", typeof(BO.Engineer.Engineer), typeof(EngineerViewWindow), new PropertyMetadata(new BO.Engineer.Engineer()));


    }
}
