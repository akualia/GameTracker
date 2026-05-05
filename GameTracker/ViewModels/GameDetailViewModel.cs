using GameTracker.Models;
using GameTracker.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GameTracker.ViewModels
{
    // Indicates that when navigating to this page with a "Game" parameter,
    // it will be assigned to the SelectedGame property
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

        // Command for adding a game to the local library
        public ICommand AddToLibraryCommand { get; }

        // Constructor with dependency injection of DatabaseService
        public GameDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddToLibraryCommand = new Command(async () => await SaveGameToLibrary());
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        // Command for navigating back to the previous page
        public ICommand GoBackCommand { get; }

        // Saves the selected game to the local database
        private async Task SaveGameToLibrary()
        {
            if (SelectedGame == null) return;

            // Save game to database
            await _databaseService.SaveGameAsync(SelectedGame);

            // Notify user and navigate back
            await App.Current.MainPage.DisplayAlert("สำเร็จ", $"เพิ่ม {SelectedGame.Name} ลงในคลังเรียบร้อย!", "ตกลง");
            await Shell.Current.GoToAsync("..");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Notifies UI when a property value changes
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}