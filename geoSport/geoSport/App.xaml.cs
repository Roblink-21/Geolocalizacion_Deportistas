using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using geoSport.Views;
using geoSport.Views.Student;

namespace geoSport
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new DashboardHome());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
