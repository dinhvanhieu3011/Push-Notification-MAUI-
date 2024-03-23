using PushNoti.Views;

namespace PushNoti
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ListPushNotification());

        }
    }
}
