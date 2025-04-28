using DataBindings.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBindings
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Person> People = new List<Person>()
        {
            new Person{Name = "Jannick", Age= 30},
            new Person{Name = "Rogue", Age= 25},
            new Person{Name = "Georgette", Age= 55},
            new Person{Name = "Gérard", Age= 68}

        };

        public MainWindow()
        {
            InitializeComponent();

            ListBoxPeople.ItemsSource = People;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = ListBoxPeople.SelectedItems;

            foreach (var item in selectedItems)
            {
                var person = item as Person;
                MessageBox.Show(person.Name);
            }
        }
    }
}
