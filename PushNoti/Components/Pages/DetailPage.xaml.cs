using Firebase.Database;
using Firebase.Database.Query;
using PushNoti.Models;

namespace PushNoti.Components.Pages;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class DetailPage : ContentPage
{
    public string ItemId
    {
        set { LoadNote(value); }
    }

    public DetailPage()
    {
        InitializeComponent();
    }

    private void LoadNote(string ItemId)
    {
        FirebaseClient firebaseClient = new FirebaseClient(baseUrl: "https://myapp-fc4db-default-rtdb.asia-southeast1.firebasedatabase.app/");
        var noteModel = firebaseClient.Child("notifications").OnceAsync<PushNotification>().Result
            .Select(item => item.Object).Where(x=>x.Id == ItemId).FirstOrDefault();
        BindingContext = noteModel;
    }
}