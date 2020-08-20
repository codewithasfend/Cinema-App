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
        public ObservableCollection<Movie> MoviesCollection;
        private int pageNumber = 0;
        public HomePage()
        {
            InitializeComponent();
            LblUserName.Text = Preferences.Get("userName", string.Empty);
            MoviesCollection = new ObservableCollection<Movie>();
            GetMovies();
        }

        private async void GetMovies()
        {
            pageNumber++;
            var movies = await ApiService.GetAllMovies(pageNumber, 5);
            foreach (var movie in movies)
            {
                MoviesCollection.Add(movie);
            }
            CvMovies.ItemsSource = MoviesCollection;
        }

        private async void TapMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            CloseHamBurgerMenu();
        }

        private void CvMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            GetMovies();
        }

        private void CvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Movie;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new MovieDetailPage(currentSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private async void CloseHamBurgerMenu()
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }

        private void TapSearch_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SearchMoviesPage());
        }

        private void TapContact_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ContactPage());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CloseHamBurgerMenu();
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("accessToken", string.Empty);
            Preferences.Set("tokenExpirationTime", 0);
            Application.Current.MainPage = new NavigationPage(new SignupPage());
        }
    }
}