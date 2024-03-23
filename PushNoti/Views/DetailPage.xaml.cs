using Firebase.Database;
using PushNoti.Models;

namespace PushNoti.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class DetailPage : ContentPage
{
    public string ItemId
    {
        set { LoadDetail(value); }
    }

    public DetailPage()
    {
        InitializeComponent();
    }

    private void LoadDetail(string id)
    {
        FirebaseClient firebaseClient = new FirebaseClient(baseUrl: "https://myapp-fc4db-default-rtdb.asia-southeast1.firebasedatabase.app/");
        var item =  firebaseClient
                .Child("notifications")
                .OnceAsync<PushNotification>().Result.Select(item => item.Object).Where(x=>x.Id == id).ToList();
        BindingContext = item;
    }
}