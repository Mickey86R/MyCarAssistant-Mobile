using System;
using Plugin.LocalNotification;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace MyCarAssistant.Droid
{
    [Activity(Label = "MyCarAssistant", Icon = "@mipmap/Icon2", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.SetStatusBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0)); // Status bar 
            Window.SetNavigationBarColor(Android.Graphics.Color.Argb(255, 0, 0, 0)); // navigationBarColor панель навигации телефона
            this.RequestedOrientation = ScreenOrientation.Portrait; // Изменение ориентации экрана

            base.OnCreate(savedInstanceState);

            NotificationCenter.CreateNotificationChannel();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}