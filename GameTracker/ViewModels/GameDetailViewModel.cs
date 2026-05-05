using GameTracker.Models;
using GameTracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GameTracker.ViewModels
{
    // QueryProperty เป็นการบอกว่าถ้านำทางมาหน้านี้พร้อมแนบคีย์ "Game" มา ให้เอามาใส่ในตัวแปร SelectedGame
    [QueryProperty(nameof(SelectedGame), "Game")]
    public class GameDetailViewModel : INotifyPropertyChanged
    {
        private Game _selectedGame;
        private readonly DatabaseService _databaseService;

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        // คำสั่งสำหรับปุ่ม Add to Library (เดี๋ยวเราจะมาเขียนระบบเซฟลง Database ทีหลัง)
        public ICommand AddToLibraryCommand { get; }
        // รับ DatabaseService เข้ามาตอนสร้างคลาส
        public GameDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddToLibraryCommand = new Command(async () => await SaveGameToLibrary());
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        public ICommand GoBackCommand { get; }
        private async Task SaveGameToLibrary()
        {
            if (SelectedGame == null) return;

            // สั่งเซฟเกมลง Database
            await _databaseService.SaveGameAsync(SelectedGame);

            // แจ้งเตือนผู้ใช้แล้วเด้งกลับหน้าเดิม
            await App.Current.MainPage.DisplayAlert("สำเร็จ", $"เพิ่ม {SelectedGame.Name} ลงในคลังเรียบร้อย!", "ตกลง");
            await Shell.Current.GoToAsync("..");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}