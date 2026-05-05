using GameTracker.ViewModels; // 👈 1. เพิ่มบรรทัดนี้เพื่อให้ไฟล์รู้จักโฟลเดอร์ ViewModels

namespace GameTracker.Views;

public partial class HomeView : ContentPage
{
    private readonly HomeViewModel _viewModel; // สร้างตัวแปรเก็บไว้

    // 👇 2. เพิ่ม HomeViewModel เข้ามาในวงเล็บตรงนี้ครับ
    public HomeView(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel; // 👈 ตอนนี้โค้ดรู้จัก viewModel แล้ว ไฟแดงจะหายไปครับ!
    }

    // 3. ฟังก์ชันนี้จะคอยดึงข้อมูล Now Playing มาโชว์ทุกครั้งที่เราเปิดหน้า Home
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDataAsync();
    }
}