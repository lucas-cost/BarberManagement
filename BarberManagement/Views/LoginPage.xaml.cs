using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace BarberManagement.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string senha = SenhaBox.Password;

            if (email == "hidaansuke@gmail.com" && senha == "1234")
            {
                // Cria a nova página principal
                var mainPage = new MainPage();

                // Acessa a janela principal (WinUI 3)
                var window = App.MainWindow;

                if (window != null)
                {
                    Frame frame = new Frame();
                    frame.Navigate(typeof(MainPage));
                    window.Content = frame;
                    window.Activate();
                }
            }
            else
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Erro",
                    Content = "Usuário ou senha inválidos.",
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
            }
        }
    }
}
