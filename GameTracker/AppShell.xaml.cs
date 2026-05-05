namespace GameTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.GameDetailView), typeof(Views.GameDetailView));
            Routing.RegisterRoute(nameof(Views.SearchView), typeof(Views.SearchView));
            Routing.RegisterRoute(nameof(Views.GameDetailView), typeof(Views.GameDetailView));
        }
    }
}
