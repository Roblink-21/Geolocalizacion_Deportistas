using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalizacionFire.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        UserRepository _userRepository = new UserRepository();
        public RegisterPage()
        {
            Title = "Registro";
            InitializeComponent();
        }

        private async void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            try
            {
                string name = TxtName.Text;
                string email = TxtEmail.Text;
                string password = TxtPassword.Text;
                string confirmPassword = TxtConfirmPass.Text;
                if (String.IsNullOrEmpty(name))
                {
                    await DisplayAlert("Warning", "Ingrese el nombre", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(email))
                {
                    await DisplayAlert("Warning", "Ingrese el correo electrónico", "Ok");
                    return;
                }
                if (password.Length < 6)
                {
                    await DisplayAlert("Warning", "La contraseña debe contener 6 dígitos.", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Ingrese la constraseña", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(confirmPassword))
                {
                    await DisplayAlert("Warning", "Ingresa la confirmación de contraseña", "Ok");
                    return;
                }
                if (password != confirmPassword)
                {
                    await DisplayAlert("Warning", "Las contraseñas no coinciden.", "Ok");
                    return;
                }

                bool isSave = await _userRepository.Register(email, name, password);
                if (isSave)
                {
                    await DisplayAlert("Registrar usuario", "Registro completado!", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Registrar usuario", "Registro fallido!", "Ok");
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("EMAIL_EXISTS"))
                {
                    await DisplayAlert("Warning", "El correo electrónico ya pertenece a otro usuario", "Ok");
                }
               

            }


        }

    }
}