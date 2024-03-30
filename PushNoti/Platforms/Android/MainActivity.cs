﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using CommunityToolkit.Mvvm.Messaging;
using PushNoti.Models;
using Plugin.Fingerprint; 

namespace PushNoti
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        internal static readonly string Channel_ID = "TestChannel";
        internal static int NotificationID = 101;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossFingerprint.SetCurrentActivityResolver(() => this);
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Android.Manifest.Permission.PostNotifications }, 1);
            }

            CreateNotificationChannel();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    if (key == "NavigationID")
                    {
                        string idValue = intent.Extras.GetString(key);
                        if (Preferences.ContainsKey("NavigationID"))
                            Preferences.Remove("NavigationID");

                        Preferences.Set("NavigationID", idValue);

                        WeakReferenceMessenger.Default.Send(new PushNotificationReceived("idValue"));
                    }
                }
            }
        }
        private void CreateNotificationChannel()
        {
            if (OperatingSystem.IsOSPlatformVersionAtLeast("android", 28))
            {
                var channel = new NotificationChannel(Channel_ID, "Test Notfication Channel", NotificationImportance.Default);

                var notificaitonManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
                notificaitonManager.CreateNotificationChannel(channel);
            }
        }

    }
}
