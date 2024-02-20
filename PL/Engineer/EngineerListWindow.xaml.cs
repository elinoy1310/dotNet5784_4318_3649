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

        public EngineerListWindow()
        {
            InitializeComponent();
       //     EngineerList=s_bl.Engineer.ReadAll();
        }
      
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        private void CbFilterByLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList=Level==BO.EngineerExperience.None? s_bl.Engineer.ReadAll():s_bl.Engineer.ReadAll(eng=>eng.level==Level); 
        }

        private void btnAddEngineer_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow().ShowDialog();
        }

        private void lvSelectEngineerToUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;
            new EngineerWindow(engineer?.Id??0).ShowDialog();
        }

        private void wLoadTheUpdatedEngineersList_Loaded(object sender, RoutedEventArgs e)
        {
            EngineerList = s_bl.Engineer.ReadAll();
        }
    }

   

}
