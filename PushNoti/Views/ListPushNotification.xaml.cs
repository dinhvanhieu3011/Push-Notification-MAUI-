using Firebase.Database;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using PushNoti.Models;
using PushNotificationDemoMAUI.Models;
using System.Collections.ObjectModel;
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
        var collection = firebaseClient
                .Child("notifications")
                .AsObservable<PushNotification>()
                .Subscribe((item) =>
                {
                    if (item.Object != null)
                    {
                        if (item.EventType == FirebaseEventType.InsertOrUpdate)
                        {
                            {
                                messages.Insert(0,item.Object);
                            }
                        }
                    }
                });
        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", "");
        }
    }

    //protected override void OnAppearing()
    //{
    //    ((Models.ListPushNotification)BindingContext).Load();
    //}

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
            // Get the note model
            var note = (Models.PushNotification)e.CurrentSelection[0];

            // Should navigate to "DetailPage?ItemId=path\on\device\XYZ.notes.txt"
            await Shell.Current.GoToAsync($"{nameof(DetailPage)}?{nameof(DetailPage.ItemId)}={note.Id}");

            // Unselect the UI
            notesCollection.SelectedItem = null;
        }
    }
}