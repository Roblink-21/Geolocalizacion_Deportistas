using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeolocalizacionFire
{
    public class UserRepository
    {
        string webAPIKey = "AIzaSyBvv8GQBj2-DPUxYkroeikBe5YamI8oeB4";
        FirebaseAuthProvider authProvider;

        public UserRepository()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(webAPIKey));
        }

        public async Task<bool> Register(string email, string name, string password)
        {

            var token = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, name);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return true;
            }
            return false;
        }

        public async Task<string> SignIn(string email, string password)
        {
            var token = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            if (!string.IsNullOrEmpty(token.FirebaseToken))
            {
                return token.FirebaseToken;
            }
            return "";
        }

        public async Task<bool> ResetPassword(string email)
        {
            await authProvider.SendPasswordResetEmailAsync(email);
            return true;
        }

        public async Task<string> ChangePassword(string token, string password)
        {
            var auth = await authProvider.ChangeUserPassword(token, password);
            return auth.FirebaseToken;
        }
    }
}

