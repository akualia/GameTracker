using System.Collections.ObjectModel;
using System.Windows.Input;
using GameTracker.Models;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class LibraryViewModel : BindableObject
    {
        private readonly DatabaseService _databaseService;
        private List<Game> _allGames = new();

        public ObservableCollection<Game> MyGames { get; set; } = new();


        private string _currentFilter = "All";
        public string CurrentFilter
        {
            get => _currentFilter;
            set { _currentFilter = value; OnPropertyChanged(nameof(CurrentFilter)); }
        }

        public ICommand FilterCommand { get; }
        public ICommand GameTappedCommand { get; }

        public LibraryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            FilterCommand = new Command<string>(FilterGames);
            GameTappedCommand = new Command<Game>(async (game) => await OnGameTapped(game));
        }

        public async Task LoadGames()
        {
            var games = await _databaseService.GetGamesAsync();
            _allGames = games ?? new List<Game>();
            FilterGames("All");
        }

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

        private async Task OnGameTapped(Game game)
        {
            if (game == null) return;

            string action = await App.Current.MainPage.DisplayActionSheet($"จัดการ: {game.Name}", "Cancel", null, "🔍 ดูรายละเอียด / แก้ไข", "🗑️ ลบเกมนี้ทิ้ง");

            if (action == "🗑️ ลบเกมนี้ทิ้ง")
            {
                bool confirm = await App.Current.MainPage.DisplayAlert("ยืนยันการลบ", $"คุณต้องการลบ {game.Name} ออกจากคลังใช่หรือไม่?", "ลบเลย", "ยกเลิก");
                if (confirm)
                {
                    await _databaseService.DeleteGameAsync(game);
                    await LoadGames();
                }
            }
            else if (action == "🔍 ดูรายละเอียด / แก้ไข")
            {
                var navParam = new Dictionary<string, object> { { "Game", game } };
                await Shell.Current.GoToAsync(nameof(Views.GameDetailView), navParam);
            }
        }
    }
}