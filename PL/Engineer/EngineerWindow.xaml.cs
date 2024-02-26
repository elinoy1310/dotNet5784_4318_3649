using System.Windows;
using System.Windows.Controls;
using BO;

namespace PL.Engineer
{

    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;

        public string State { get; init; }
        public EngineerWindow(int idEngineer = 0)
        {
            InitializeComponent();
            if (idEngineer == 0)
            {
                add_updateEngineer = new BO.Engineer();
                State = "Add";
            }
            else
            { 
                State = "Update";
                try
                {
                    add_updateEngineer = s_bl.Engineer.Read(idEngineer);                   
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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





        public TaskInEngineer task
        {
            get { return (TaskInEngineer)GetValue(taskProperty); }
            set { SetValue(taskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for task.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty taskProperty =
            DependencyProperty.Register("task", typeof(TaskInEngineer), typeof(EngineerWindow), new PropertyMetadata(0));





        private void AddOrUpdateClick(object sender, RoutedEventArgs e)
        {
            if (State == "Add")
            {
                try
                {
                    s_bl.Engineer.Create(add_updateEngineer); 
                    MessageBoxResult successMsg = MessageBox.Show("The engineer created successfully!");
                }
                catch (BO.BlAlreadyExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                catch (BO.BlWrongDataException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
              
            }
            else
            {
                try
                {
                    s_bl.Engineer.Update(add_updateEngineer);
                    MessageBoxResult successMsg = MessageBox.Show("The engineer updated successfully!");
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              
                }
                catch (BO.BlWrongDataException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
       
                }
                catch (BO.BlCannotBeUpdatedException ex)
                {
                    MessageBoxResult mbResult = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
               
                }
                
            }

            this.Close();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
