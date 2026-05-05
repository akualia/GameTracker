using System.Collections.ObjectModel;
using System.Windows.Input;
using GameTracker.Models;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class LibraryViewModel : BindableObject
    {
        private readonly DatabaseService _databaseService;

        // Stores all games loaded from the database
        private List<Game> _allGames = new();

        // Collection bound to the UI
        public ObservableCollection<Game> MyGames { get; set; } = new();

        // Current filter state (e.g., All, Playing, Completed, etc.)
        private string _currentFilter = "All";
        public string CurrentFilter
        {
            get => _currentFilter;
            set { _currentFilter = value; OnPropertyChanged(nameof(CurrentFilter)); }
        }

        // Command to filter games by status
        public ICommand FilterCommand { get; }

        // Command triggered when a game is tapped
        public ICommand GameTappedCommand { get; }

        public LibraryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            FilterCommand = new Command<string>(FilterGames);
            GameTappedCommand = new Command<Game>(async (game) => await OnGameTapped(game));
        }

        // Loads games from the database and applies default filter
        public async Task LoadGames()
        {
            var games = await _databaseService.GetGamesAsync();
            _allGames = games ?? new List<Game>();
            FilterGames("All");
        }

        // Filters games based on selected status
        private void FilterGames(string status)
        {

            CurrentFilter = status;

            MyGames.Clear();
            var filtered = status == "All"
                ? _allGames
                : _allGames.Where(g => g.PlayStatus == status).ToList();

            foreach (var game in filtered)
                MyGames.Add(game);
        }

        // Handles user interaction when a game is tapped
        private async Task OnGameTapped(Game game)
        {
            if (game == null) return;

            // Show action options to the user
            string action = await App.Current.MainPage.DisplayActionSheet($"จัดการ: {game.Name}", "Cancel", null, "🔍 ดูรายละเอียด / แก้ไข", "🗑️ ลบเกมนี้ทิ้ง");

            if (action == "🗑️ ลบเกมนี้ทิ้ง")
            {
                // Confirm deletion
                bool confirm = await App.Current.MainPage.DisplayAlert("ยืนยันการลบ", $"คุณต้องการลบ {game.Name} ออกจากคลังใช่หรือไม่?", "ลบเลย", "ยกเลิก");
                if (confirm)
                {
                    await _databaseService.DeleteGameAsync(game);
                    await LoadGames(); // Refresh list after deletion
                }
            }
            else if (action == "🔍 ดูรายละเอียด / แก้ไข")
            {
                // Navigate to GameDetailView with selected game
                var navParam = new Dictionary<string, object> { { "Game", game } };
                await Shell.Current.GoToAsync(nameof(Views.GameDetailView), navParam);
            }
        }
    }
}