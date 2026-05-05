using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class StatsView : ContentPage
{
    private readonly StatsViewModel _viewModel;

    public StatsView(StatsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // ฟังก์ชันนี้จะทำงานทุกครั้งที่ผู้ใช้เปิดหน้านี้ขึ้นมา
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadStatsAsync();
    }
}