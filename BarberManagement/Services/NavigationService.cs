using BarberManagement.Views;
using Microsoft.UI.Xaml.Controls;

namespace BarberManagement.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame _frame;

        public NavigationService(Frame frame)
        {
            _frame = frame;
        }

        public void NavigateToMainPage()
        {
            _frame.Navigate(typeof(MainPage));
        }

        public void NavigateToRegisterPage()
        {
            _frame.Navigate(typeof(RegisterPage));
        }

        public void NavigateToLoginPage()
        {
            _frame.Navigate(typeof(LoginPage));
        }
    }
}