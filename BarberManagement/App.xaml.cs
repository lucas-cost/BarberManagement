using BarberManagement.Data;
using BarberManagement.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BarberManagement
{
    public partial class App : Application
    {
        public static Window? MainAppWindow { get; private set; }

        public App()
        {
            this.InitializeComponent();

            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated(); 
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            MainAppWindow = new Window();
            var rootFrame = new Frame();
            MainAppWindow.Content = rootFrame;
            rootFrame.Navigate(typeof(LoginPage));
            MainAppWindow.Activate();
        }
    }
}