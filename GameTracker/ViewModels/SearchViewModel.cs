using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using GameTracker.Models;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly RawgApiService _apiService;
        private string _searchQuery;
        private bool _isLoading;

        // Collection that holds search results and updates the UI automatically
        public ObservableCollection<Game> SearchResults { get; set; } = new ObservableCollection<Game>();

        // User input from the search bar
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(nameof(SearchQuery)); }
        }

        // Indicates whether data is currently being loaded (used for loading indicators)
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        // Command triggered when the search button is pressed
        public ICommand SearchCommand { get; }

        // Command triggered when a game is selected
        public ICommand GameTappedCommand { get; }

        // Command to navigate back to the previous page
        public ICommand GoBackCommand { get; }

        public SearchViewModel(RawgApiService apiService)
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            _apiService = apiService;
            SearchCommand = new Command(async () => await PerformSearch());
            GameTappedCommand = new Command<Game>(async (game) => await GoToGameDetail(game));
        }

        // Performs search using the API and updates the results list
        private async Task PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            IsLoading = true;
            SearchResults.Clear();   // Clear previous results

            // Fetch data from API
            var results = await _apiService.SearchGamesAsync(SearchQuery);

            // Populate the collection with new results
            foreach (var game in results)
            {
                SearchResults.Add(game);
            }

            IsLoading = false;
        }

        // Navigates to GameDetailView with the selected game
        private async Task GoToGameDetail(Game selectedGame)
        {
            if (selectedGame == null) return;

            // Create navigation parameters
            var navigationParameter = new Dictionary<string, object>
    {
        { "Game", selectedGame }
    };

            // Navigate to GameDetailView with the selected game
            await Shell.Current.GoToAsync(nameof(Views.GameDetailView), navigationParameter);
        }

        // Notifies UI when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    }

}