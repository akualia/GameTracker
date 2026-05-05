using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class StatsView : ContentPage
{
    private readonly StatsViewModel _viewModel;

    // Constructor with dependency injection of StatsViewModel
    public StatsView(StatsViewModel viewModel)
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

        // Load statistics data when the page becomes visible
        await _viewModel.LoadStatsAsync();
    }
}