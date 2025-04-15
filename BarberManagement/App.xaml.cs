using BarberManagement.Data;
using BarberManagement.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Diagnostics;

namespace BarberManagement
{
    public partial class App : Application
    {
        public static Window? MainAppWindow { get; private set; }
        public static Frame? RootFrame { get; private set; }

        public App()
        {
            Debug.WriteLine("Construtor App iniciado...");
            try
            {
                InitializeComponent();
                Debug.WriteLine("InitializeComponent concluído.");

                // Inicializar banco
                try
                {
                    using (var db = new AppDbContext())
                    {
                        Debug.WriteLine("Tentando criar o banco...");
                        db.Database.EnsureCreated();
                        Debug.WriteLine("Banco criado com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Erro ao criar o banco: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em InitializeComponent: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Debug.WriteLine("OnLaunched iniciado...");
            try
            {
                MainAppWindow = new Window();
                if (MainAppWindow == null)
                {
                    Debug.WriteLine("Erro: Janela principal é nula.");
                    return;
                }

                MainAppWindow.AppWindow.Resize(new Windows.Graphics.SizeInt32(900, 700));
                MainAppWindow.AppWindow.Move(new Windows.Graphics.PointInt32(100, 100));

                RootFrame = new Frame();
                MainAppWindow.Content = RootFrame;
                RootFrame.Navigate(typeof(LoginPage), null, new EntranceNavigationTransitionInfo());
                Debug.WriteLine("Navegação para LoginPage concluída.");

                MainAppWindow.Activate();
                Debug.WriteLine("Janela ativada.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro em OnLaunched: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}