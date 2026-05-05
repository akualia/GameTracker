using GameTracker.ViewModels;

namespace GameTracker.Views;

public partial class SearchView : ContentPage
{
    public SearchView(SearchViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // บอกว่าหน้านี้จะใช้ข้อมูลจาก ViewModel นี้
    }
}