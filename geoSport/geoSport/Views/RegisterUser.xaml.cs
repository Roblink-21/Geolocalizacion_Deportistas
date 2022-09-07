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
    public partial class RegisterUser : ContentPage
    {

        UserRepository _userRepository = new UserRepository();
        public RegisterUser()
        {
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
                    await DisplayAlert("Warning", "Ingrese el correo", "Ok");
                    return;
                }
                if (password.Length<6)
                {
                    await DisplayAlert("Warning", "La contraseña debe contener 6 dígitos.", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(password))
                {
                    await DisplayAlert("Warning", "Ingrese la contraseña", "Ok");
                    return;
                }
                if (String.IsNullOrEmpty(confirmPassword))
                {
                    await DisplayAlert("Warning", "Confirme la contraseña", "Ok");
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
                    await DisplayAlert("Register user", "Registro completo", "Ok");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Register user", "Registration failed", "Ok");
                }
            }
            catch(Exception exception)
            {
               if(exception.Message.Contains("EMAIL_EXISTS"))
                {
                   await DisplayAlert("Warning", "El correo electrónico ya está en uso", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", exception.Message, "Ok");
                }
                
            }
            

        }   
    }
}