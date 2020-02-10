using MyBooks.Models;
using MyBooks.ViewModels.Base;
using MyBooks.Views;
using Prism.AppModel;
using Prism.Commands;
using Prism.Navigation;
using SQLite;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyBooks.ViewModels
{
    public class BooksViewModel : ViewModelBase, IPageLifecycleAware
    {
        public ICommand NewBookCommand { get; set; }
        public ICommand BookDetailsCommand { get; set; }
        public ObservableCollection<BestBook> Books { get; set; }

        readonly INavigationService _navigationService;
        public BooksViewModel(INavigationService navigationService)
        {
            Books = new ObservableCollection<BestBook>();
            _navigationService = navigationService;
            NewBookCommand = new DelegateCommand(NewBookAction);
            BookDetailsCommand = new DelegateCommand<BestBook>(BookDetail, CanGoToBookDetails);
            ReadBooks();
        }

        private bool CanGoToBookDetails(BestBook selectedBook)
        {
            return selectedBook != null;
        }

        private async void BookDetail(BestBook selectedBook)
        {
            await _navigationService.NavigateAsync(nameof(BookDetailsView), GetBookDetailsParameters(selectedBook));
        }

        private NavigationParameters GetBookDetailsParameters(BestBook selectedBook)
        {
            var parameters = new NavigationParameters();
            parameters.Add(nameof(selectedBook), selectedBook);
            return parameters;
        }

        private void ReadBooks()
        {
            using (var conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<BestBook>();
                var books = conn.Table<BestBook>().ToList();
                Books.Clear();
                books?.ForEach(book => Books.Add(book));
            }
        }

        private async void NewBookAction()
        {
            await _navigationService.NavigateAsync(nameof(NewBookView));
        }

        public void OnAppearing()
        {
            ReadBooks();
        }

        public void OnDisappearing()
        {

        }
    }
}
