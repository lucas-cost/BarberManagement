using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BarberManagement.Views
{
    public sealed partial class MainPage :Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Dashboard",
                Content = "Aqui será o painel de visualização de dados.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
    }
}
