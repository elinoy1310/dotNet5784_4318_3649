using BO;
using PL.Engineer;
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
    /// Interaction logic for UserEntryWindow.xaml
    /// </summary>
    public partial class UserEntryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public UserEntryWindow()
        {
            InitializeComponent();
            MyUser = new BO.User();
        }



        public BO.User MyUser
        {
            get { return (BO.User)GetValue(MyUserProperty); }
            set { SetValue(MyUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyUserProperty =
            DependencyProperty.Register("MyUser", typeof(BO.User), typeof(UserEntryWindow), new PropertyMetadata(null));



        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.UserType type = s_bl.User.ReadType(MyUser.UserId, MyUser.passWord ?? " ");
                switch (type)
                {
                    case BO.UserType.Manager:
                        new ManagerWindow(MyUser.UserId).Show();
                        break;
                    case BO.UserType.Engineer:
                        new EngineerViewWindow(MyUser.UserId).Show();
                        break;

                }
            }
            catch (BlWrongDataException ex)
            {
                MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void pbGetPassword(object sender, RoutedEventArgs e)
        {
            if ((sender as PasswordBox) is not null)
                MyUser.passWord =(sender as PasswordBox)!.Password ;
        }
    }
}
