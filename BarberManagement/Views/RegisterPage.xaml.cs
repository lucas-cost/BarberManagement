using BarberManagement.Services;
using BarberManagement.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace BarberManagement.Views
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterViewModel ViewModel { get; private set; }

        public RegisterPage()
        {
            Debug.WriteLine("Inicializando RegisterPage...");
            try
            {
                var frame = (Frame)App.MainAppWindow?.Content;
                var navigationService = new NavigationService(frame);
                ViewModel = new RegisterViewModel(navigationService);
                DataContext = ViewModel;
                InitializeComponent();
                Debug.WriteLine("InitializeComponent concluído.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em InitializeComponent: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}