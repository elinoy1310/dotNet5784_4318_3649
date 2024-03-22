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


        /// <summary>
        /// Initializes a new instance of the EngineerWindow class.
        /// </summary>
        /// <param name="idEngineer">The ID of the engineer. Defaults to 0.</param>
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


        /// <summary>
        /// Gets or sets the engineer being added or updated in this window.
        /// </summary>
        public BO.Engineer add_updateEngineer
        {
            get { return (BO.Engineer)GetValue(add_updateEngineerProperty); }
            set { SetValue(add_updateEngineerProperty, value); }
        }

        /// <summary>
        /// Identifies the add_updateEngineer dependency property.
        /// </summary>
        public static readonly DependencyProperty add_updateEngineerProperty =
            DependencyProperty.Register("add_updateEngineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(new BO.Engineer()));



        /// <summary>
        /// Handles the click event of the "Add" or "Update" button.
        /// Performs the corresponding action based on the window state.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
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
            else // State is "Update"
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
  
    }
}
