using GeolocalizacionFire.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GeolocalizacionFire.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}