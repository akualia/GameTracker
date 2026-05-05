using System.ComponentModel;
using System.Linq;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class StatsViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;

        private int _totalGames;
        private int _playingGames;
        private int _completedGames;
        private double _averageRating;

        // Total number of games in the library
        public int TotalGames
        {
            get => _totalGames;
            set { _totalGames = value; OnPropertyChanged(nameof(TotalGames)); }
        }

        // Number of games currently being played
        public int PlayingGames
        {
            get => _playingGames;
            set { _playingGames = value; OnPropertyChanged(nameof(PlayingGames)); }
        }

        // Number of completed games
        public int CompletedGames
        {
            get => _completedGames;
            set { _completedGames = value; OnPropertyChanged(nameof(CompletedGames)); }
        }

        // Average rating of all games
        public double AverageRating
        {
            get => _averageRating;
            set { _averageRating = value; OnPropertyChanged(nameof(AverageRating)); }
        }

        public StatsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Calculates statistics based on stored games
        public async Task LoadStatsAsync()
        {
            var games = await _databaseService.GetGamesAsync();

            if (games != null && games.Any())
            {
                TotalGames = games.Count;
                PlayingGames = games.Count(g => g.PlayStatus == "Playing");
                CompletedGames = games.Count(g => g.PlayStatus == "Completed");

                // Calculate average rating (rounded to 1 decimal place)
                AverageRating = Math.Round(games.Average(g => g.Rating), 1);
            }
            else
            {
                // Set all values to 0 if no games exist
                TotalGames = 0;
                PlayingGames = 0;
                CompletedGames = 0;
                AverageRating = 0;
            }
        }

        // Notifies UI when a property value changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}