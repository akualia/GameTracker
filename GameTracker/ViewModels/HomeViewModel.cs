using System.Collections.ObjectModel;
using System.Windows.Input;
using GameTracker.Models;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class HomeViewModel : BindableObject
    {
        private readonly DatabaseService _databaseService;

        // Collections used to display games on the home screen
        public ObservableCollection<Game> NowPlayingGames { get; set; } = new();
        public ObservableCollection<Game> RecentlyAddedGames { get; set; } = new();

        // Command triggered when tapping the search bar
        public ICommand GoToSearchCommand { get; }

        // Command triggered when selecting a game
        public ICommand SelectGameCommand { get; }

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Navigate to SearchView
            GoToSearchCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Views.SearchView)));

            // Navigate to GameDetailView with selected game as parameter
            SelectGameCommand = new Command<Game>(async (game) =>
            {
                if (game == null) return;

                var navParam = new Dictionary<string, object>
                {
                    { "Game", game }
                };
                await Shell.Current.GoToAsync(nameof(Views.GameDetailView), navParam);
            });
        }

        // Loads data from the database and updates UI collections
        public async Task LoadDataAsync()
        {
            var allGames = await _databaseService.GetGamesAsync();

            NowPlayingGames.Clear();
            RecentlyAddedGames.Clear();

            if (allGames != null && allGames.Any())
            {
                // Get only games with "Playing" status
                var playing = allGames.Where(g => g.PlayStatus == "Playing").ToList();
                foreach (var g in playing) NowPlayingGames.Add(g);

                // Get most recently added games (top 5 by Id)
                var recent = allGames.OrderByDescending(g => g.Id).Take(5).ToList();
                foreach (var g in recent) RecentlyAddedGames.Add(g);
            }
        }
    }
}