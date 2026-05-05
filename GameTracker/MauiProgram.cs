using GameTracker.Services;
using GameTracker.Views;
using Microsoft.Extensions.Logging;
namespace GameTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // --- ลงทะเบียน Services (ระบบหลังบ้าน) ---
            builder.Services.AddSingleton<Services.RawgApiService>();
            builder.Services.AddSingleton<Services.DatabaseService>();

            // --- ลงทะเบียน ViewModels (ตัวจัดการข้อมูล) ---
            builder.Services.AddTransient<ViewModels.SearchViewModel>();
            builder.Services.AddTransient<ViewModels.GameDetailViewModel>();
            builder.Services.AddTransient<ViewModels.LibraryViewModel>();
            builder.Services.AddTransient<ViewModels.StatsViewModel>();
            builder.Services.AddTransient<ViewModels.HomeViewModel>(); 

            // --- ลงทะเบียน Views (หน้าจอ UI) ---
            builder.Services.AddTransient<Views.SearchView>();
            builder.Services.AddTransient<Views.GameDetailView>();
            builder.Services.AddTransient<Views.LibraryView>();
            builder.Services.AddTransient<Views.StatsView>();
            builder.Services.AddTransient<Views.HomeView>();
            return builder.Build();
        }
    }
}
