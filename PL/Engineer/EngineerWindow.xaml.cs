using System.Windows;
using System.Windows.Controls;

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

        private void AddOrUpdateClick(object sender, RoutedEventArgs e)
        {
           Button temp= (Button)sender;
            string? strValue = temp.Content.ToString();
            if (strValue == "Add")
            {
                try
                {
                    s_bl.Engineer.Create(add_updateEngineer);
                }
                catch (BO.BlAlreadyExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new EngineerListWindow().Show();
                    }
                }
                catch (BO.BlWrongDataException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new EngineerListWindow().Show();
                    }
                }
                MessageBoxResult successMsg = MessageBox.Show("The engineer created successfully!");
            }
            else
            {
                try
                {
                    s_bl.Engineer.Update(add_updateEngineer);
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new EngineerListWindow().Show();
                    }
                }
                catch (BO.BlWrongDataException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new EngineerListWindow().Show();
                    }
                }
                catch (BO.BlCannotBeUpdatedException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    if (mbResult == MessageBoxResult.OK)
                    {
                        new EngineerListWindow().Show();
                    }
                }
                MessageBoxResult successMsg = MessageBox.Show("The engineer updated successfully!");
            }
            this.Close();
        }
    }
}
