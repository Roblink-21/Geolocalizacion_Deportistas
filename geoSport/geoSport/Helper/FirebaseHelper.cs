using System;
using System.Collections.Generic;
using System.Text;

using geoSport.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace geoSport.Helper
{
    internal class FirebaseHelper
    {
        FirebaseClient firebase = new FirebaseClient("https://geolocalitationathletes-default-rtdb.firebaseio.com/");

        public async Task<List<Person>> GetAllPersons()
        {

            return (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Select(item => new Person
              {
                  Name = item.Object.Name,
                  PersonId = item.Object.PersonId,
                  Longitud = item.Object.Longitud,
                  Latitud = item.Object.Latitud,
                  url = item.Object.Latitud.ToString() + "," + item.Object.Longitud.ToString()

              }).ToList();
        }

        public async Task AddPerson(int personId, string name, double age, double age2)
        {

            await firebase
              .Child("Persons")
              .PostAsync(new Person() { PersonId = personId, Name = name, Longitud = age, Latitud = age2});
        }

        public async Task<Person> GetPerson(int personId)
        {
            var allPersons = await GetAllPersons();
            await firebase
              .Child("Persons")
              .OnceAsync<Person>();
            return allPersons.Where(a => a.PersonId == personId).FirstOrDefault();
        }

        public async Task UpdatePerson(int personId, string name, double age, double age2)
        {
            var toUpdatePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();

            await firebase
              .Child("Persons")
              .Child(toUpdatePerson.Key)
              .PutAsync(new Person() { PersonId = personId, Name = name, Longitud = age, Latitud = age2 });
        }

        public async Task DeletePerson(int personId)
        {
            var toDeletePerson = (await firebase
              .Child("Persons")
              .OnceAsync<Person>()).Where(a => a.Object.PersonId == personId).FirstOrDefault();
            await firebase.Child("Persons").Child(toDeletePerson.Key).DeleteAsync();

        }
    }
}
