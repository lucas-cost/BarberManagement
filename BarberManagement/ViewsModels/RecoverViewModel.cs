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
    public partial class RecoverViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        private readonly INavigationService _navigationService;

        public RecoverViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private async Task RecoverAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    await ShowErrorDialogAsync("Por favor, digite um e-mail.");
                    return;
                }

                using var db = new AppDbContext();
                var usuario = db.Users.FirstOrDefault(u => u.Email == email);

                if (usuario == null)
                {
                    await ShowErrorDialogAsync("E-mail não encontrado.");
                    return;
                }

                // Simula o envio de e-mail de recuperação
                System.Diagnostics.Debug.WriteLine($"Simulando envio de e-mail de recuperação para {Email}");
                await ShowSuccessDialogAsync("Instruções de recuperação enviadas para seu e-mail.");

                // Volta para a LoginPage após sucesso
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