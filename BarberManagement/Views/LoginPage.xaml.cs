using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using BarberManagement.Data;
using System;
using System.Linq;

namespace BarberManagement.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Entrar_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string senha = SenhaBox.Password;

            using var db = new AppDbContext();

            var usuario = db.Users.FirstOrDefault(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                // Login bem-sucedido
                var frame = (Frame)App.MainAppWindow!.Content;

                if (frame != null)
                {
                    frame.Navigate(typeof(MainPage));
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
                await dialog.ShowAsync();
            }
        }
    }
}
