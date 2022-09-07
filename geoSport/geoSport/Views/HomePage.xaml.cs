using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using geoSport.Views.Student;
using geoSport.Helper;

namespace geoSport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        StudentRepository user = new StudentRepository();
        FirebaseHelper firebaseHelper = new FirebaseHelper();
        

        public HomePage()
        {
            InitializeComponent();
            LblUser.Text = Preferences.Get("userEmail", "default");
            //getUser();
            


        }

        private void Btn_lop(object sender, EventArgs e)
        {
            getUser();
        }


        private async void getUser()
        {
            
            string email = Preferences.Get("userEmail", "default");

            var person = await user.GetPerson(email);

            LblID.Text = person.Name.ToString();


        }

        private async void addUser()
        {
            string email = Preferences.Get("userEmail", "default");
            
            await firebaseHelper.AddPerson(3, email, 0, 0);
        }



        private void BtnStudentList_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentListPage());
        }

        private void butLocation_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Location());

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