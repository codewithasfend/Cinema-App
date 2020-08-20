using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnixTimeStamp;
using Xamarin.Essentials;

namespace RealWorldApp.Services
{
    public static class ApiService
    {
        public static async Task<bool> RegisterUser(string name, string email, string password)
        {
            var register = new Register()
            {
                Name = name,
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/register", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<bool> Login(string email, string password)
        {
            var login = new Login()
            {
                Email = email,
                Password = password
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/users/login", content);
            if (!response.IsSuccessStatusCode) return false;
            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Token>(jsonResult);
            Preferences.Set("accessToken", result.access_token);
            Preferences.Set("userId", result.user_id);
            Preferences.Set("userName", result.user_Name);
            Preferences.Set("tokenExpirationTime", result.expiration_Time);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            return true;
        }

        public static async Task<List<MovieList>> GetAllMovies(int pageNumber, int pageSize)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + String.Format("api/movies/AllMovies?pageNumber={0}&pageSize={1}", pageNumber, pageSize));
            return JsonConvert.DeserializeObject<List<MovieList>>(response);
        }


        public static async Task<bool> DeleteMovie(int movieId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + "api/movies/" + movieId);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public static async Task<List<Reservation>> GetAllReservations()
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/reservations");
            return JsonConvert.DeserializeObject<List<Reservation>>(response);
        }

        public static async Task<ReservationDetail> GetReservationDetail(int reservationId)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + "api/reservations/" + reservationId);
            return JsonConvert.DeserializeObject<ReservationDetail>(response);
        }


        public static async Task<bool> AddMovie(MediaFile mediaFile,Movie movie)
        {
            await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var content = new MultipartFormDataContent
            {
                {new StringContent(movie.Name),"Name" },
                {new StringContent(movie.Description),"Description" },
                {new StringContent(movie.Language),"Language" },
                {new StringContent(movie.Duration),"Duration" },
                {new StringContent(movie.PlayingDate),"PlayingDate" },
                {new StringContent(movie.PlayingTime),"PlayingTime" },
                {new StringContent(movie.TicketPrice.ToString()),"TicketPrice" },
                {new StringContent(movie.Rating.ToString()),"Rating" },
                {new StringContent(movie.Genre),"Genre" },
                {new StringContent(movie.TrailorUrl),"TrailorUrl" },
            };
            content.Add(new StreamContent(new MemoryStream(movie.ImageArray)), "Image",mediaFile.Path);
            var response = await httpClient.PostAsync(AppSettings.ApiUrl + "api/movies", content);
            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

    }

    public static class TokenValidator
    {
        public static async Task CheckTokenValidity()
        {
            var expirationTime = Preferences.Get("tokenExpirationTime", 0);
            Preferences.Set("currentTime", UnixTime.GetCurrentTime());
            var currentTime = Preferences.Get("currentTime", 0);
            if (expirationTime < currentTime)
            {
                var email = Preferences.Get("email", string.Empty);
                var password = Preferences.Get("password", string.Empty);
                await ApiService.Login(email, password);
            }
        }
    }
}
