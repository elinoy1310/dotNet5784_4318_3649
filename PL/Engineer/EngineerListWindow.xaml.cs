using Accessibility;
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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.EngineerExperience Level { get; set; } = BO.EngineerExperience.None;


        /// <summary>
        /// Initializes a new instance of the EngineerListWindow class.
        /// </summary>
        public EngineerListWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets or sets the list of engineers displayed in this window.
        /// </summary>
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        /// <summary>
        /// Identifies the EngineerList dependency property.
        /// </summary>
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


        /// <summary>
        /// Handles the selection changed event of the filter by level combo box.
        /// Updates the EngineerList property based on the selected level.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void CbFilterByLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = Level == BO.EngineerExperience.None ? s_bl.Engineer.ReadAll() : s_bl.Engineer.ReadAll(eng => eng.level == Level);
        }

        /// <summary>
        /// Handles the click event of the "Add Engineer" button.
        /// Opens a new EngineerWindow to add a new engineer.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void btnAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }


        /// <summary>
        /// Handles the mouse double-click event of the engineer selection list view.
        /// Opens a new EngineerWindow to update the selected engineer upon double-click.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void lvSelectEngineerToUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            new EngineerWindow(engineer?.Id ?? 0).ShowDialog();
            EngineerList = s_bl?.Engineer.ReadAll()!;
        }


        /// <summary>
        /// Handles the Loaded event of the window to load the updated list of engineers.
        /// Sets the EngineerList property to the list of engineers retrieved from the business logic layer.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event data.</param>
        private void wLoadTheUpdatedEngineersList_Loaded(object sender, RoutedEventArgs e)
        {
            EngineerList = s_bl.Engineer.ReadAll();
        }
    }

}
