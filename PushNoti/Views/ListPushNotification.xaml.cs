using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using PushNoti.Models;
using PushNotificationDemoMAUI.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text;

namespace PushNoti.Views;

public partial class ListPushNotification : ContentPage
{
    FirebaseClient firebaseClient = new FirebaseClient(baseUrl: "https://myapp-fc4db-default-rtdb.asia-southeast1.firebasedatabase.app/");
    public ObservableCollection<PushNotification> messages { get; set; } = new ObservableCollection<PushNotification>();
    public string _deviceToken;
    public ListPushNotification()
    {
        InitializeComponent();
        BindingContext = this;
        FirstLoad();
        Load();
        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", "");
        }
    }

    private void FirstLoad()
    {
        var list = firebaseClient
                     .Child("notifications")
                     .OrderByKey().LimitToLast(10)
                     .OnceAsync<PushNotification>().Result.Select(x => x.Object).OrderByDescending(x=>x.CreatedDate).ToList().Take(10);
        foreach (var item in list)
        {
            messages.Add(item);
        }
    }

    private void Load()
    {
        var collection = firebaseClient
        .Child("notifications")
        .OrderByKey().LimitToLast(10)
        .AsObservable<PushNotification>()
        .Subscribe((item) =>
        {
            if (item.Object != null)
            {
                messages.Insert(0, item.Object);
                if (messages.Count > 10)
                    messages.RemoveAt(9);
            }
        });
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        var androidNotificationObject = new Dictionary<string, string>();
        androidNotificationObject.Add("NavigationID", "2");
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

    private async void notesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            if (e.CurrentSelection.FirstOrDefault() is not PushNotification item)
                return;
            await Navigation.PushAsync(new DetailPage{
                BindingContext = item
            });
        }
    }
}