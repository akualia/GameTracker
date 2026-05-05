using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class HomeView : ContentPage
{
    private readonly HomeViewModel _viewModel; 

    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();

        // Set ViewModel via dependency injection and bind it to the UI
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Refresh data every time the page appears
        await _viewModel.LoadDataAsync();
    }
}