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
    public partial class MainPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient(baseUrl: "https://myapp-fc4db-default-rtdb.asia-southeast1.firebasedatabase.app/");
       public ObservableCollection<PushNotification> messages { get; set; } = new ObservableCollection<PushNotification>();

        int count = 0;
        private string _deviceToken;
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            var collection = firebaseClient
                    .Child("notifications")
                    .AsObservable<PushNotification>()
                    .Subscribe((item) =>
                    {
                        if (item.Object != null)
                        {
                            messages.Insert(0,item.Object);
                        }
                    });

            WeakReferenceMessenger.Default.Register<PushNotificationReceived>(this, (r, m) =>
            {
                string msg = m.Value;
                NavigateToPage();
            });

            if (Preferences.ContainsKey("DeviceToken"))
            {
                _deviceToken = Preferences.Get("DeviceToken", "");
            }

           // ReadFireBaseAdminSdk();
            NavigateToPage();
        }

        private async void ReadFireBaseAdminSdk()
        {
            var stream = await FileSystem.OpenAppPackageFileAsync("google-services.json");
            var reader = new StreamReader(stream);

            var jsonContent = reader.ReadToEnd();


            if (FirebaseMessaging.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonContent)
                });
            }
        }
        private void NavigateToPage()
        {

            if (Preferences.ContainsKey("NavigationID"))
            {
                string id = Preferences.Get("NavigationID", "");
                //if (id == "1")
                //{
                //    AppShell.Current.GoToAsync(nameof(NewPage1));
                //}
                //if (id == "2")
                //{
                //    AppShell.Current.GoToAsync(nameof(NewPage2));
                //}
                //Preferences.Remove("NavigationID");
            }
        }
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var androidNotificationObject = new Dictionary<string, string>();
            androidNotificationObject.Add("NavigationID", "2");

            var iosNotificationObject = new Dictionary<string, object>();
            iosNotificationObject.Add("NavigationID", "2");

            var pushNotificationRequest = new PushNotificationRequest
            {
                notification = new NotificationMessageBody
                {
                    title = "Notification Title",
                    body = "Notification body"
                },
                data = androidNotificationObject,
                registration_ids = new List<string> { _deviceToken }
            };

            //var messageList = new List<Message>();

            //var obj = new Message
            //{
            //    Token = _deviceToken,
            //    Notification = new Notification
            //    {
            //        Title = "Tilte",
            //        Body = "message body"
            //    },
            //    Data = androidNotificationObject,
            //    Apns = new ApnsConfig()
            //    {
            //        Aps = new Aps
            //        {
            //            Badge = 15,
            //            CustomData = iosNotificationObject,
            //        }
            //    }
            //};

            //messageList.Add(obj);

            //var response = await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);

            string url = "https://fcm.googleapis.com/fcm/send";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAAIxVuBHU:APA91bHfmUmwyZir7LEJislHhWwylzZk2dvkAwm7W7CbfKZbaRJBjQfpPPTLknTG6mTu6zctDwSYzNL1HrtgS8WPhcyeRsHgv31eZRFtghF3i-mRqS6A5lrNCqhZKJV75EMFB0Haxg3k");

                string serializeRequest = JsonConvert.SerializeObject(pushNotificationRequest);
                var response = await client.PostAsync(url, new StringContent(serializeRequest, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    await App.Current.MainPage.DisplayAlert("Notification sent", "notification sent", "OK");
                }
            }
        }

    }
}
