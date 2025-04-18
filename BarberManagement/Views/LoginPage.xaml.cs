﻿using BarberManagement.Services;
using BarberManagement.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace BarberManagement.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginViewModel ViewModel { get; private set; }

        public LoginPage()
        {
            Debug.WriteLine("Inicializando LoginPage...");
            try
            {
                var frame = (Frame)App.MainAppWindow?.Content!;
                var navigationService = new NavigationService(frame);
                ViewModel = new LoginViewModel(navigationService);
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