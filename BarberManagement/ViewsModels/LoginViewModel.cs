using BarberManagement.Data;
using BarberManagement.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using BarberManagement.Views;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Media;

namespace BarberManagement.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string senha = string.Empty;

        [RelayCommand]
        private async Task LoginAsync(object parameter = null)
        {
            ContentDialog dialogHost = parameter as ContentDialog ?? new ContentDialog();

            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                await ShowErrorDialog(dialogHost, "Por favor, preencha todos os campos.");
                return;
            }

            using var db = new AppDbContext();
            var usuario = db.Users.FirstOrDefault(u => u.Email == Email);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(Senha, usuario.Senha))
            {
                var frame = (Frame)App.MainAppWindow!.Content;
                frame?.Navigate(typeof(MainPage));
            }
            else
            {
                await ShowErrorDialog(dialogHost, "Usuário ou senha inválidos.");
            }
        }

        [RelayCommand]
        private async Task RegisterAsync(object parameter = null)
        {
            ContentDialog dialogHost = parameter as ContentDialog ?? new ContentDialog();

            var emailBox = new TextBox
            {
                PlaceholderText = "E-mail",
                Margin = new Thickness(0, 0, 0, 12),
                Style = Application.Current.Resources["ModernTextBoxStyle"] as Style
            };
            var senhaBox = new PasswordBox
            {
                PlaceholderText = "Senha",
                Margin = new Thickness(0, 0, 0, 12),
                Style = Application.Current.Resources["ModernPasswordBoxStyle"] as Style
            };
            var isAdminCheck = new CheckBox
            {
                Content = "É administrador?",
                Margin = new Thickness(0, 0, 0, 12),
                FontSize = 14
            };
            var stackPanel = new StackPanel { Spacing = 12 };
            stackPanel.Children.Add(new TextBlock
            {
                Text = "Cadastrar Usuário",
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 16),
                HorizontalAlignment = HorizontalAlignment.Center
            });
            stackPanel.Children.Add(emailBox);
            stackPanel.Children.Add(senhaBox);
            stackPanel.Children.Add(isAdminCheck);

            var dialog = new ContentDialog
            {
                Title = "",
                Content = stackPanel,
                PrimaryButtonText = "Cadastrar",
                PrimaryButtonStyle = Application.Current.Resources["ModernButtonStyle"] as Style,
                CloseButtonText = "Cancelar",
                CloseButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style,
                XamlRoot = App.MainAppWindow!.Content.XamlRoot,
                Background = Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"] as Brush,
                CornerRadius = new CornerRadius(12)
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string email = emailBox.Text.Trim();
                string senha = senhaBox.Password;
                bool isAdmin = isAdminCheck.IsChecked == true;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    await ShowErrorDialog(dialogHost, "Por favor, preencha todos os campos.");
                    return;
                }

                using var db = new AppDbContext();
                if (db.Users.Any(u => u.Email == email))
                {
                    await ShowErrorDialog(dialogHost, "Este e-mail já está cadastrado.");
                    return;
                }

                var novoUsuario = new User
                {
                    Email = email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(senha),
                    IsAdmin = isAdmin
                };

                db.Users.Add(novoUsuario);
                await db.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Usuário cadastrado: {email}");

                await ShowSuccessDialog(dialogHost, "Usuário cadastrado com sucesso!");
            }
        }

        private static async Task ShowErrorDialog(ContentDialog dialogHost, string message)
        {
            dialogHost.Title = "Erro";
            dialogHost.Content = message;
            dialogHost.CloseButtonText = "OK";
            dialogHost.Background = Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"] as Brush;
            dialogHost.CornerRadius = new CornerRadius(12);
            dialogHost.XamlRoot = App.MainAppWindow!.Content.XamlRoot;
            await dialogHost.ShowAsync();
        }

        private static async Task ShowSuccessDialog(ContentDialog dialogHost, string message)
        {
            dialogHost.Title = "Sucesso";
            dialogHost.Content = message;
            dialogHost.CloseButtonText = "OK";
            dialogHost.Background = Application.Current.Resources["SystemControlBackgroundChromeMediumBrush"] as Brush;
            dialogHost.CornerRadius = new CornerRadius(12);
            dialogHost.XamlRoot = App.MainAppWindow!.Content.XamlRoot;
            await dialogHost.ShowAsync();
        }
    }
}