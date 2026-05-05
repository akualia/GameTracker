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

        public int TotalGames
        {
            get => _totalGames;
            set { _totalGames = value; OnPropertyChanged(nameof(TotalGames)); }
        }

        public int PlayingGames
        {
            get => _playingGames;
            set { _playingGames = value; OnPropertyChanged(nameof(PlayingGames)); }
        }

        public int CompletedGames
        {
            get => _completedGames;
            set { _completedGames = value; OnPropertyChanged(nameof(CompletedGames)); }
        }

        public double AverageRating
        {
            get => _averageRating;
            set { _averageRating = value; OnPropertyChanged(nameof(AverageRating)); }
        }

        public StatsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // ฟังก์ชันสำหรับคำนวณสถิติ
        public async Task LoadStatsAsync()
        {
            var games = await _databaseService.GetGamesAsync();

            if (games != null && games.Any())
            {
                TotalGames = games.Count;
                PlayingGames = games.Count(g => g.PlayStatus == "Playing");
                CompletedGames = games.Count(g => g.PlayStatus == "Completed");

                // หาค่าเฉลี่ยของเรตติ้ง 
                AverageRating = Math.Round(games.Average(g => g.Rating), 1);
            }
            else
            {
                // ถ้ายังไม่มีเกมเลย ให้เซ็ตเป็น 0
                TotalGames = 0;
                PlayingGames = 0;
                CompletedGames = 0;
                AverageRating = 0;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}