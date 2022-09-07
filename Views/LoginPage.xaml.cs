using GeolocalizacionFire.ViewModels;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace GeolocalizacionFire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public ICommand TapCommand => new Command(async () => await Navigation.PushModalAsync(new RegisterPage()));

        public LoginPage()
        {
            InitializeComponent();
            bool hasKey = Preferences.ContainsKey("token");
            if (hasKey)
            {
                string token = Preferences.Get("token", "");
                if (!string.IsNullOrEmpty(token))
                {
                    Navigation.PushAsync(new AboutPage());
                }
            }
        }
        private async void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Ingrese el correo electrócnico", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Ingrese la contraseña", "Ok");
                    return;
                }
                string token = await _userRepository.SignIn(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    Preferences.Set("token", token);
                    Preferences.Set("userEmail", email);
                    await Navigation.PushAsync(new AboutPage());
                }
                else
                {
                    await DisplayAlert("Iniciar sesión", "Inicio de sesión fallido", "ok");
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("EMAIL_NOT_FOUND"))
                {
                    await DisplayAlert("No autorizado", "Correo no encontrado", "ok");
                }
                else if (exception.Message.Contains("INVALID_PASSWORD"))
                {
                    await DisplayAlert("No autorizado", "Contraseña incorrecta", "ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "ok");
                }
            }

        }

        private async void RegisterTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterPage());
        }

        private async void SignOut(object sender, EventArgs e)
        {
            Preferences.Clear();

           
        }
       /* 
        * private async void ForgotTap_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPasswordPage());
        }*/
    }
}
