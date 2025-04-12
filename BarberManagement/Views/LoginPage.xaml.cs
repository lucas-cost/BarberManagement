using BarberManagement.Data;
using BarberManagement.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                await ShowErrorDialog("Por favor, preencha todos os campos.");
                return;
            }

            using var db = new AppDbContext();
            var usuario = db.Users.FirstOrDefault(u => u.Email == email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
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
                await ShowErrorDialog("Usuário ou senha inválidos.");
            }
        }

        private async void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            // Criar o formulário de cadastro
            var emailBox = new TextBox { PlaceholderText = "E-mail", Margin = new Thickness(0, 0, 0, 10) };
            var senhaBox = new PasswordBox { PlaceholderText = "Senha", Margin = new Thickness(0, 0, 0, 10) };
            var isAdminCheck = new CheckBox { Content = "É administrador?", Margin = new Thickness(0, 0, 0, 10) };
            var stackPanel = new StackPanel { Spacing = 10 };
            stackPanel.Children.Add(emailBox);
            stackPanel.Children.Add(senhaBox);
            stackPanel.Children.Add(isAdminCheck);

            var dialog = new ContentDialog
            {
                Title = "Cadastrar Usuário",
                Content = stackPanel,
                PrimaryButtonText = "Cadastrar",
                CloseButtonText = "Cancelar",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string email = emailBox.Text.Trim();
                string senha = senhaBox.Password;
                bool isAdmin = isAdminCheck.IsChecked == true;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    await ShowErrorDialog("Por favor, preencha todos os campos.");
                    return;
                }

                using var db = new AppDbContext();
                if (db.Users.Any(u => u.Email == email))
                {
                    await ShowErrorDialog("Este e-mail já está cadastrado.");
                    return;
                }

                User novoUsuario = new User
                {
                    Email = email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(senha),
                    IsAdmin = isAdmin
                };

                db.Users.Add(novoUsuario);
                await db.SaveChangesAsync();

                await ShowSuccessDialog("Usuário cadastrado com sucesso!");
            }
        }

        private async Task ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Erro",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task ShowSuccessDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "Sucesso",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}