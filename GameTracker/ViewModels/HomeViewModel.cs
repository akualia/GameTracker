using System.Collections.ObjectModel;
using System.Windows.Input;
using GameTracker.Models;
using GameTracker.Services;

namespace GameTracker.ViewModels
{
    public class HomeViewModel : BindableObject
    {
        private readonly DatabaseService _databaseService;

        // รายการเกมที่จะเอาไปโชว์ในหน้าจอ
        public ObservableCollection<Game> NowPlayingGames { get; set; } = new();
        public ObservableCollection<Game> RecentlyAddedGames { get; set; } = new();

        // คำสั่งเมื่อกดช่องค้นหา
        public ICommand GoToSearchCommand { get; }
        public ICommand SelectGameCommand { get; }

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;


            GoToSearchCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(Views.SearchView)));

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

        public async Task LoadDataAsync()
        {
            var allGames = await _databaseService.GetGamesAsync();

            NowPlayingGames.Clear();
            RecentlyAddedGames.Clear();

            if (allGames != null && allGames.Any())
            {
                // 1. ดึงเฉพาะเกมที่มีสถานะ "Playing"
                var playing = allGames.Where(g => g.PlayStatus == "Playing").ToList();
                foreach (var g in playing) NowPlayingGames.Add(g);

                // 2. ดึงเกมที่เพิ่งเพิ่มล่าสุด 
                var recent = allGames.OrderByDescending(g => g.Id).Take(5).ToList();
                foreach (var g in recent) RecentlyAddedGames.Add(g);
            }
        }
    }
}