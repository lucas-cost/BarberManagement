using BarberManagement.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

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
            _frame.Navigate(typeof(MainPage), null, new EntranceNavigationTransitionInfo());
        }

        public void NavigateToRegisterPage()
        {
            _frame.Navigate(typeof(RegisterPage), null, new EntranceNavigationTransitionInfo());
        }

        public void NavigateToLoginPage()
        {
            _frame.Navigate(typeof(LoginPage), null, new EntranceNavigationTransitionInfo());
        }

        public void NavigateToRecoverPage()
        {
            _frame.Navigate(typeof(RecoverPage), null, new EntranceNavigationTransitionInfo());
        }
    }
}