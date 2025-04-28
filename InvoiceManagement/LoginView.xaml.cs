using System;
using System.Windows;
using System.Windows.Controls;

namespace InvoiceManagement
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        public void OnLoginBtnClicked(object sender, RoutedEventArgs e)
        {
            string passwordEntered = PasswordBox.Password;

            var envPw = Environment.GetEnvironmentVariable("InvoiceManagement");

            if (envPw != null)
            {
                if (passwordEntered == envPw)
                {
                    // The correct password is : Test123

                    //MessageBox.Show("Entered correct password !");

                    // Transition vers la vue Profile
                    // Rechercher le ContentControl principal (MainContent dans MainWindow)
                    MainWindow parentWindow = Window.GetWindow(this) as MainWindow;
                    if (parentWindow != null)
                    {
                        parentWindow.NavigateTo(new ProfileView());
                    }
                }
                else
                {

                    MessageBox.Show("Login failed !");
                }
            }
            else
            {
                MessageBox.Show("Environnement variable not found !!!!");
            }
        }

        public void OnPasswordChanged(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = !string.IsNullOrEmpty(PasswordBox.Password);

        }
    }
}
