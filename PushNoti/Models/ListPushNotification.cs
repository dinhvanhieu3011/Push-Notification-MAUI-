using Firebase.Database;
using FirebaseAdmin.Messaging;
using PushNoti.Models;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace PushNoti.Models;

internal class ListPushNotification
{        

    public ObservableCollection<PushNotification> messages { get; set; } = new ObservableCollection<PushNotification>();
    public bool isLoad = false;
    public ListPushNotification() => Load();

    public void Load()
    {
        //if (!isLoad) 
        //{
        //    messages.Clear();
        //}
        messages.Clear();
        isLoad = true; 
        FirebaseClient firebaseClient = new FirebaseClient(baseUrl: "https://myapp-fc4db-default-rtdb.asia-southeast1.firebasedatabase.app/");

        var collection = firebaseClient
                    .Child("notifications")
                    .AsObservable<PushNotification>()
                    .Subscribe((item) =>
                    {
                        if (item.Object != null)
                        {
                            messages.Insert(0, item.Object);
                        }
                    });
    }
}