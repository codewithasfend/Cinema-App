using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMoviesPage : ContentPage
    {
        public SearchMoviesPage()
        {
            InitializeComponent();
        }

        private async void EntSearchMovie_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                return; 
            }
            var moviesList = await ApiService.FindMovies(e.NewTextValue.ToLower());
            CvMovies.ItemsSource = moviesList;
        }

        private void CvMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as FindMovie;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new MovieDetailPage(currentSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}