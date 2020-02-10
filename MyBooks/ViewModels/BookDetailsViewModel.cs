using MyBooks.Models;
using MyBooks.Repositories;
using MyBooks.ViewModels.Base;
using Prism.Commands;
using Prism.Navigation;
using SQLite;
using System.Windows.Input;

namespace MyBooks.ViewModels
{
    public class BookDetailsViewModel : ViewModelBase, INavigatedAware
    {
        private BestBook _bestBook;

        public BestBook BestBook
        {
            get { return _bestBook; }
            set
            {
                _bestBook = value;
                OnPropertyChanged();
            }
        }


        public ICommand DeleteBookCommand { get; set; }

        private readonly INavigationService _navigationService;

        public BookDetailsViewModel()
        {
            DeleteBookCommand = new DelegateCommand(DeleteBook);
        }

        private async void DeleteBook()
        {
            var connection = new SQLiteAsyncConnection(App.DatabasePath);
            var bestBookRepository = new Repository<BestBook>(connection);

            var isDeleted = await bestBookRepository.Delete(BestBook);
            if (isDeleted)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Book deleted?", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error happened while deleting a book, please try again.", "Ok");
            }

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            BestBook = parameters["selectedBook"] as BestBook;
        }


    }
}
