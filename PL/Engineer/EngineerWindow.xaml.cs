using System.Windows;

namespace PL.Engineer
{

    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;
        public EngineerWindow(int idEngineer = 0)
        {
            InitializeComponent();
            if (idEngineer == 0)
            {
                add_updateEngineer = new BO.Engineer();
            }
            else
            {
                try
                {
                    add_updateEngineer = s_bl.Engineer.Read(idEngineer);
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK) 
                    {
                        new EngineerListWindow().Show();
                    }
                }

            }
        }


        public BO.Engineer add_updateEngineer
        {
            get { return (BO.Engineer)GetValue(add_updateEngineerProperty); }
            set { SetValue(add_updateEngineerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for add_updateEngineer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty add_updateEngineerProperty =
            DependencyProperty.Register("add_updateEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(new BO.Engineer()));

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
