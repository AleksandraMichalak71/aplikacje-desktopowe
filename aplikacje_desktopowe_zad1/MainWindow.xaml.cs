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

namespace aplikacje_desktopowe_zad1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Zamieniamy przewijanie w pionie na poziome, bo przy obsłudze touchpadem
        // (gest przesuwania pionowego) naturalniej jest przewijać poziomy panel w ten sposób.
        private void TopScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            TopScrollViewer.ScrollToHorizontalOffset(TopScrollViewer.HorizontalOffset - e.Delta);
            e.Handled = true;
        }
    }
}