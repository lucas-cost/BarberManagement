using BarberManagement.Data;
using BarberManagement.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BarberManagement.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        private readonly INavigationService _navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    await ShowErrorDialogAsync("Por favor, preencha todos os campos.");
                    return;
                }

                using var db = new AppDbContext();
                var usuario = db.Users.FirstOrDefault(u => u.Email == Email);

                if (usuario != null && BCrypt.Net.BCrypt.Verify(Password, usuario.Senha))
                {
                    _navigationService.NavigateToMainPage();
                }
                else
                {
                    await ShowErrorDialogAsync("Usuário ou senha inválidos.");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"Erro: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Register()
        {
            _navigationService.NavigateToRegisterPage();
        }

        [RelayCommand]
        private void Recover()
        {
            _navigationService.NavigateToRecoverPage();
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
    }
}