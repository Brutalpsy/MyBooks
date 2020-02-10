using MyBooks.ViewModels;
using MyBooks.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyBooks
{
    public partial class App : PrismApplication
    {
        public static string DatabasePath;
        public App(IPlatformInitializer initializer = null) : base(initializer)
        {

        }

        public App(string databasePath, IPlatformInitializer initializer = null) : base(initializer)
        {
            DatabasePath = databasePath;
            NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(BooksView)}");
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BooksView, BooksViewModel>();
            containerRegistry.RegisterForNavigation<NewBookView, NewBookViewModel>();
            containerRegistry.RegisterForNavigation<BookDetailsView, BookDetailsViewModel>();
        }
    }
}
