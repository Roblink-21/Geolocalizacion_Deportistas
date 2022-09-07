using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Plugin.Geolocator;
using System.Threading.Tasks;
using Xamarin.Forms;
using geoSport.Helper;
using geoSport.Model;
using Xamarin.Essentials;

namespace geoSport
{
    public partial class MainPage : ContentPage
    {
        FirebaseHelper firebaseHelper = new FirebaseHelper();

        double lat;
        double lng;

        public MainPage()
        {
            InitializeComponent();
            OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                MessagingCenter.Subscribe<LocationMessage>(this, "Location", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        locationLabel.Text += $"{Environment.NewLine}{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}";
                        lat = message.Latitude;
                        lng = message.Longitude;

                        getLoc();
                        
                        Console.WriteLine($"{message.Latitude}, {message.Longitude}, {DateTime.Now.ToLongTimeString()}");
                    });
                });

                MessagingCenter.Subscribe<StopServiceMessage>(this, "ServiceStopped", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        locationLabel.Text = "Location Service has been stopped!";
                    });
                });

                MessagingCenter.Subscribe<LocationErrorMessage>(this, "LocationError", message => {
                    Device.BeginInvokeOnMainThread(() => {
                        locationLabel.Text = "There was an error updating location!";
                    });
                });

                if (Preferences.Get("LocationServiceRunning", false) == true)
                {
                    StartService();
                }
            }

            
        }

        public async void getLoc()
        {
            await firebaseHelper.UpdatePerson(1, "Roberth", lng, lat);
            OnAppearing();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
            freshDate.Text = "Datos actualizados: " + DateTime.Now.ToLongTimeString();

            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {

            await firebaseHelper.AddPerson(3, "mi", lng, lat);
           

            await DisplayAlert("Success", "Person Added Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private void Button_url(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var GeoPosi = button.ClassId;

            string url = "https://maps.google.com/?ll=" + GeoPosi.ToString() + "&z=18&t=k";

            //urlt.Text = url.ToString();
            Device.OpenUri(new Uri(url));
        }


        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var permission = await Xamarin.Essentials.Permissions.RequestAsync<Xamarin.Essentials.Permissions.LocationAlways>();

            if (permission == Xamarin.Essentials.PermissionStatus.Denied)
            {
                // TODO Let the user know they need to accept
                return;
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (CrossGeolocator.Current.IsListening)
                {
                    await CrossGeolocator.Current.StopListeningAsync();
                    CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;

                    return;
                }

                await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 10, false, new Plugin.Geolocator.Abstractions.ListenerSettings
                {
                    ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = false,
                    DeferralDistanceMeters = 10,
                    DeferralTime = TimeSpan.FromSeconds(5),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = true
                });

                CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                if (Preferences.Get("LocationServiceRunning", false) == false)
                {
                    StartService();
                    noti.Text = "Detener";
                }
                else
                {
                    StopService();
                    noti.Text = "Empezar";
                }
            }
        }

        private void StartService()
        {
            var startServiceMessage = new StartServiceMessage();
            MessagingCenter.Send(startServiceMessage, "ServiceStarted");
            Preferences.Set("LocationServiceRunning", true);
            locationLabel.Text = "Location Service has been started!";
        }

        private void StopService()
        {
            var stopServiceMessage = new StopServiceMessage();
            MessagingCenter.Send(stopServiceMessage, "ServiceStopped");
            Preferences.Set("LocationServiceRunning", false);
        }

        private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            locationLabel.Text += $"{e.Position.Latitude}, {e.Position.Longitude}, {e.Position.Timestamp.TimeOfDay}{Environment.NewLine}";;

            Console.WriteLine($"{e.Position.Latitude}, {e.Position.Longitude}, {e.Position.Timestamp.TimeOfDay}");
        }

    }
}
