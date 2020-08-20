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
    public partial class ReservationListPage : ContentPage
    {
        public ObservableCollection<Reservation> ReservationsCollection;
        public ReservationListPage()
        {
            InitializeComponent();
            ReservationsCollection = new ObservableCollection<Reservation>();
            GetReservationsList();
        }

        private async void GetReservationsList()
        {
            var reservations = await ApiService.GetAllReservations();
            foreach (var reservation in reservations)
            {
                ReservationsCollection.Add(reservation);   
            }
            CvReservations.ItemsSource = ReservationsCollection;
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CvReservations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Reservation;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ReservationDetailPage(currentSelection.Id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}