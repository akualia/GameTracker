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

        // ตัวแปรนี้จะเก็บรายชื่อเกมที่ค้นหาเจอ และนำไปโชว์บนหน้าจออัตโนมัติ
        public ObservableCollection<Game> SearchResults { get; set; } = new ObservableCollection<Game>();

        // ข้อความที่ผู้ใช้พิมพ์ในช่องค้นหา
        public string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; OnPropertyChanged(nameof(SearchQuery)); }
        }

        // สถานะตอนกำลังโหลดข้อมูล (เพื่อโชว์ไอคอนหมุนๆ)
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        // คำสั่งเมื่อกดปุ่มค้นหา
        public ICommand SearchCommand { get; }
        public ICommand GameTappedCommand { get; }
        public ICommand GoBackCommand { get; }

        public SearchViewModel(RawgApiService apiService)
        {
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
            _apiService = apiService;
            SearchCommand = new Command(async () => await PerformSearch());
            GameTappedCommand = new Command<Game>(async (game) => await GoToGameDetail(game));
        }

        private async Task PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            IsLoading = true;
            SearchResults.Clear(); 

            // ดึงข้อมูลจาก API
            var results = await _apiService.SearchGamesAsync(SearchQuery);

            // นำข้อมูลที่ได้มาใส่ใน List
            foreach (var game in results)
            {
                SearchResults.Add(game);
            }

            IsLoading = false;
        }
        private async Task GoToGameDetail(Game selectedGame)
        {
            if (selectedGame == null) return;

            // สร้างข้อมูลที่จะส่งข้ามหน้าจอ
            var navigationParameter = new Dictionary<string, object>
    {
        { "Game", selectedGame }
    };

            // สั่งนำทางไปยังหน้า GameDetailView พร้อมแนบข้อมูลเกมไป
            await Shell.Current.GoToAsync(nameof(Views.GameDetailView), navigationParameter);
        }

        // โค้ดส่วนนี้ช่วยให้หน้าจออัปเดตอัตโนมัติเมื่อข้อมูลเปลี่ยน
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



    }

}