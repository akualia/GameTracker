using GameTracker.Services;
using GameTracker.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.Extensions.Configuration;
namespace GameTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // --- Register Services (backend logic, API, database) ---
            builder.Services.AddSingleton<Services.RawgApiService>();
            builder.Services.AddSingleton<Services.DatabaseService>();

            // --- Register ViewModels (application state & logic) ---
            builder.Services.AddTransient<ViewModels.SearchViewModel>();
            builder.Services.AddTransient<ViewModels.GameDetailViewModel>();
            builder.Services.AddTransient<ViewModels.LibraryViewModel>();
            builder.Services.AddTransient<ViewModels.StatsViewModel>();
            builder.Services.AddTransient<ViewModels.HomeViewModel>();

            // --- Register Views (UI pages) ---
            builder.Services.AddTransient<Views.SearchView>();
            builder.Services.AddTransient<Views.GameDetailView>();
            builder.Services.AddTransient<Views.LibraryView>();
            builder.Services.AddTransient<Views.StatsView>();
            builder.Services.AddTransient<Views.HomeView>();

            // Splash screen page (kept as singleton)
            builder.Services.AddSingleton<SplashPage>();
            
            return builder.Build();
        }
    }
}
