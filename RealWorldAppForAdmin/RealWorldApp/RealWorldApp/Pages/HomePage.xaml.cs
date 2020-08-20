using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void BtnMovies_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new MoviesListPage());
        }

        private void BtnReservations_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ReservationListPage());
        }
    }
}