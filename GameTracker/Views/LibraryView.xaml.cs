using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class LibraryView : ContentPage
{
    private readonly LibraryViewModel _viewModel;

    public LibraryView(LibraryViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;

        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadGames();
    }
}