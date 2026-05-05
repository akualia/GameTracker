using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class LibraryView : ContentPage
{
    private readonly LibraryViewModel _viewModel;

    // Constructor with dependency injection of LibraryViewModel
    public LibraryView(LibraryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;

        // Set BindingContext for data binding in XAML
        BindingContext = _viewModel;
    }

    // Called when the page appears on screen
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load games from the database when the page becomes visible
        await _viewModel.LoadGames();
    }
}