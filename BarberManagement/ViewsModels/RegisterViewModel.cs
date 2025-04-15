using BarberManagement.Data;
using BarberManagement.Models;
using BarberManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarberManagement.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool isAdmin;

        private readonly INavigationService _navigationService;

        public RegisterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    await ShowErrorDialogAsync("Por favor, preencha todos os campos.");
                    return;
                }

                using var db = new AppDbContext();
                if (db.Users.Any(u => u.Email == email))
                {
                    await ShowErrorDialogAsync("Este e-mail já está cadastrado.");
                    return;
                }

                var novoUsuario = new User
                {
                    Email = email,
                    Senha = BCrypt.Net.BCrypt.HashPassword(password),
                    IsAdmin = isAdmin
                };

                db.Users.Add(novoUsuario);
                await db.SaveChangesAsync();
                await ShowSuccessDialogAsync("Usuário cadastrado com sucesso!");
                _navigationService.NavigateToLoginPage();
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"Erro: {ex.Message}");
            }
        }

        [RelayCommand]
        private void BackToLogin()
        {
            _navigationService.NavigateToLoginPage();
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            var xamlRoot = App.MainAppWindow?.Content?.XamlRoot;
            if (xamlRoot == null)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: XamlRoot é nulo. Mensagem: {message}");
                return;
            }

            var dialog = new ContentDialog
            {
                Title = "Erro",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task ShowSuccessDialogAsync(string message)
        {
            var xamlRoot = App.MainAppWindow?.Content?.XamlRoot;
            if (xamlRoot == null)
            {
                System.Diagnostics.Debug.WriteLine($"Erro: XamlRoot é nulo. Mensagem: {message}");
                return;
            }

            var dialog = new ContentDialog
            {
                Title = "Sucesso",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}