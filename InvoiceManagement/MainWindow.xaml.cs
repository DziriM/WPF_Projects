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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InvoiceManagement
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NavigateTo(new LoginView());
        }

        public void NavigateTo(UserControl newView)
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));

            fadeOut.Completed += (s, e) =>
            {
                MainContent.Content = newView;

                var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));

                MainContent.BeginAnimation(OpacityProperty, fadeIn);
            };

            MainContent.BeginAnimation(OpacityProperty, fadeOut);

        }
    }
}
