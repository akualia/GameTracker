using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class GameDetailView : ContentPage
{
    public GameDetailView(GameDetailViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}