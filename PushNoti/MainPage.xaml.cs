using CommunityToolkit.Mvvm.Messaging;
using PushNoti.Models;
using PushNotificationDemoMAUI.Models;
using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Firebase.Database;
using System.Collections.ObjectModel;

namespace PushNoti
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.DetailPage), typeof(Views.DetailPage));

        }
    }
}
