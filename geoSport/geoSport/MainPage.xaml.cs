using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            getLoc();
        }

        public async void getLoc()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            lat = location.Latitude;
            lng = location.Longitude;
            

        }

        protected async override void OnAppearing()
        {

            base.OnAppearing();
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnAdd_Clicked(object sender, EventArgs e)
        {

            await firebaseHelper.AddPerson(Convert.ToInt32(txtId.Text), txtName.Text, lng, lat);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
           

            await DisplayAlert("Success", "Person Added Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnRetrive_Clicked(object sender, EventArgs e)
        {
            var person = await firebaseHelper.GetPerson(Convert.ToInt32(txtId.Text));
            if (person != null)
            {
                txtId.Text = person.PersonId.ToString();
                txtName.Text = person.Name;
    
                await DisplayAlert("Success", "Person Retrive Successfully", "OK");

            }
            else
            {
                await DisplayAlert("Success", "No Person Available", "OK");
            }

        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.UpdatePerson(Convert.ToInt32(txtId.Text), txtName.Text, lng, lat);
            txtId.Text = string.Empty;
            txtName.Text = string.Empty;
           
            
            await DisplayAlert("Success", "Person Updated Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            await firebaseHelper.DeletePerson(Convert.ToInt32(txtId.Text));
            await DisplayAlert("Success", "Person Deleted Successfully", "OK");
            var allPersons = await firebaseHelper.GetAllPersons();
            lstPersons.ItemsSource = allPersons;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string url = "https://www.google.com.ec/maps/@" + lng + "," + lat;

            urlt.Text = url.ToString();
            //Device.OpenUri(new Uri(url));
        }
    }
}
