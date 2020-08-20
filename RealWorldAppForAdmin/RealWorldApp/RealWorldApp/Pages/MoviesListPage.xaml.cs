using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviesListPage : ContentPage
    {
        public ObservableCollection<MovieList> MoviesCollection;
        private int pageNumber = 0;
        public MoviesListPage()
        {
            InitializeComponent();
            MoviesCollection = new ObservableCollection<MovieList>();
            GetMovies();
        }
        private async void GetMovies()
        {
            pageNumber++;
            var movies = await ApiService.GetAllMovies(pageNumber, 6);
            foreach (var movie in movies)
            {
                MoviesCollection.Add(movie);
            }
            CvMovies.ItemsSource = MoviesCollection;
        }

        private void CvMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            GetMovies();
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void CvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as MovieList;
            if (currentSelection == null) return;
            var result = await DisplayAlert("Alert", "Do you want to delete this movie", "Yes", "No");
            if (result)
            {
                var response = await ApiService.DeleteMovie(currentSelection.Id);
                if (response == false) return;
                MoviesCollection.Clear();
                pageNumber = 0;
                GetMovies();
            }
            
            ((CollectionView)sender).SelectedItem = null;
        }

        private void ImgAdd_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddMoviePage());
        }
    }
}