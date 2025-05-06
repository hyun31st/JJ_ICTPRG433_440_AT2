using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JJ_ICTPRG433_440_AT2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lbl_MainWindowDate.Content = "Today: " + DateTime.Now.ToLongDateString();

            
        }

        private void AssignContractorToAJob_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("xxx");
        }
    }
}