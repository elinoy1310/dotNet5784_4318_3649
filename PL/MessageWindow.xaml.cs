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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        string code;
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public MessageWindow()
        {
            InitializeComponent();
            code = "";
        }

        private void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(sender as PasswordBox is not null)
            code = (sender as PasswordBox)!.Password;
        }


        public bool ShowError
        {
            get { return (bool)GetValue(ShowErrorProperty); }
            set { SetValue(ShowErrorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowError.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowErrorProperty =
            DependencyProperty.Register("ShowError", typeof(bool), typeof(MessageWindow), new PropertyMetadata(false));


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (code == "WED34!")
            {
                try
                {
                    s_bl!.User.Create(new BO.User() { UserId = 325984318, UserType = BO.UserType.Manager, passWord = "eli2812" });
                }
                catch (BlAlreadyExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                try
                {
                    s_bl!.User.Create(new BO.User() { UserId = 213203649, UserType = BO.UserType.Manager, passWord = "hadar0203" });

                }
                catch (BlAlreadyExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                this.Close();
                MessageBox.Show("the users was initialize succesfuly", "Access Granted", MessageBoxButton.OK);
            }
            else
                ShowError = true;
        }

        private void Enter_Click(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) 
                btnOK_Click(sender,new RoutedEventArgs());
        }
    }
}
