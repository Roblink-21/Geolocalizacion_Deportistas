using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace geoSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardHome : ContentPage
    {
        public DashboardHome()
        {
            InitializeComponent();
        }
        private async void SingTap_Tapped(object sender, EventArgs e)
        {
           //await Navigation.PushModalAsync(new LoginPage());
           await this.Navigation.PushAsync(new NavigationPage(new LoginPage()));
        }

        private async void RegisterTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterUser());

        }

    }



}