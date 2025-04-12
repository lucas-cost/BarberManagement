using BarberManagement.Data;
using BarberManagement.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.UI.Text;

namespace BarberManagement.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            Debug.WriteLine("Inicializando LoginPage...");
            try
            {
                InitializeComponent();
                Debug.WriteLine("InitializeComponent concluído.");

                // Verificar controles
                if (EmailBox == null)
                    Debug.WriteLine("Erro: EmailBox é nulo.");
                if (SenhaBox == null)
                    Debug.WriteLine("Erro: SenhaBox é nulo.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em InitializeComponent: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private async void Entrar_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Entrar_Click chamado...");
            try
            {
                if (EmailBox == null || SenhaBox == null)
                {
                    Debug.WriteLine("Erro: EmailBox ou SenhaBox não encontrados.");
                    await ShowErrorDialog("Erro interno: Controles não encontrados.");
                    return;
                }

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
                    Debug.WriteLine($"Login bem-sucedido: {email}");
                    var frame = (Frame)App.MainAppWindow!.Content;
                    if (frame != null)
                    {
                        frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        Debug.WriteLine("Erro: Frame é nulo.");
                        await ShowErrorDialog("Erro interno: Navegação não disponível.");
                    }
                }
                else
                {
                    await ShowErrorDialog("Usuário ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em Entrar_Click: {ex.Message}\n{ex.StackTrace}");
                await ShowErrorDialog($"Erro: {ex.Message}");
            }
        }

        private async void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Cadastrar_Click chamado...");
            try
            {
                var emailBox = new TextBox
                {
                    PlaceholderText = "E-mail",
                    Margin = new Thickness(0, 0, 0, 10),
                    Style = Application.Current.Resources["ModernTextBoxStyle"] as Style
                };
                var senhaBox = new PasswordBox
                {
                    PlaceholderText = "Senha",
                    Margin = new Thickness(0, 0, 0, 10),
                    Style = Application.Current.Resources["ModernPasswordBoxStyle"] as Style
                };
                var isAdminCheck = new CheckBox
                {
                    Content = "É administrador?",
                    Margin = new Thickness(0, 0, 0, 10)
                };
                var stackPanel = new StackPanel { Spacing = 10 };
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

                    var novoUsuario = new User
                    {
                        Email = email,
                        Senha = BCrypt.Net.BCrypt.HashPassword(senha),
                        IsAdmin = isAdmin
                    };

                    db.Users.Add(novoUsuario);
                    await db.SaveChangesAsync();
                    Debug.WriteLine($"Usuário cadastrado: {email}");

                    await ShowSuccessDialog("Usuário cadastrado com sucesso!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em Cadastrar_Click: {ex.Message}\n{ex.StackTrace}");
                await ShowErrorDialog($"Erro: {ex.Message}");
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