using GeolocalizacionFire.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GeolocalizacionFire.ViewModels
{
    public class RegisterUserViewModel 
    {
        public Command RegisterCommand { get; }
        public RegisterUserViewModel()
        {
            RegisterCommand = new Command(OnRegisterClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}