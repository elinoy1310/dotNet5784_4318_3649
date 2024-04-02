using BO;
using DalApi;
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
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public StartWindow()
        {
            InitializeComponent();
            CurrentTime= s_bl.Clock;
            //s_bl!.User.Create(new BO.User() { UserId = 325984318, UserType = BO.UserType.Manager, passWord = "eli2812" });
            //s_bl!.User.Create(new BO.User() { UserId = 213203649, UserType = BO.UserType.Manager, passWord = "hadar0203" });

        }


        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(StartWindow), new PropertyMetadata(null));

        private void btnAddHour_click(object sender, RoutedEventArgs e)
        {
            s_bl.PromoteTime(BO.Time.Hour);
            CurrentTime = s_bl.Clock;
        }

        private void btnAddDay_click(object sender, RoutedEventArgs e)
        {
            s_bl.PromoteTime(BO.Time.Day);
            CurrentTime = s_bl.Clock;
        }

        private void btnAddYear_click(object sender, RoutedEventArgs e)
        {
            s_bl.PromoteTime(BO.Time.Year);
            CurrentTime = s_bl.Clock;
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            new UserEntryWindow().Show();
        }

        private void btnInitUseres_Click(object sender, RoutedEventArgs e)
        {
           new MessageWindow().ShowDialog();
        }
    }
}
