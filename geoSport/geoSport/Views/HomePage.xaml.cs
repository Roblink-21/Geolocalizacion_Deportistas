using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using geoSport.Views.Student;

namespace geoSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();

        public HomePage()
        {
            InitializeComponent();
            LblUser.Text = Preferences.Get("userEmail", "default");
        }

        private void BtnStudentList_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentListPage());
        }

        private void BtnChangePassword_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ChangePassword());
        }

        private async void BtnLogout_Clicked(object sender, EventArgs e)
        {

            bool isSave = await _userRepository.Signout();
            if (isSave)
            {
                await DisplayAlert("Register user", "Salir completed", "Ok");
                //await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Register user", "Salir failed", "Ok");
            }
        }
    }
}