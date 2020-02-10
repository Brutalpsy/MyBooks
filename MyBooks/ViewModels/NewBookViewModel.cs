using MyBooks.Models;
using MyBooks.Repositories;
using MyBooks.ViewModels.Base;
using MyBooks.ViewModels.Helpers;
using Prism.Commands;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MyBooks.ViewModels
{
    public class NewBookViewModel : ViewModelBase
    {
        public ICommand SearchCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ObservableCollection<BestBook> Books { get; set; }

        public NewBookViewModel()
        {
            Books = new ObservableCollection<BestBook>();
            SearchCommand = new DelegateCommand<string>(GetSearchResults);
            SaveCommand = new DelegateCommand<BestBook>(SaveBook, CanSaveBook);
        }

        private bool CanSaveBook(BestBook selectedBook)
        {
            return selectedBook != null;
        }

        private async void SaveBook(BestBook book)
        {
            var connection = new SQLiteAsyncConnection(App.DatabasePath);
            var bestBookRepository = new Repository<BestBook>(connection);

            var isInserted = await bestBookRepository.Insert(book);
            if (isInserted)
            {
                await App.Current.MainPage.DisplayAlert("Success", "Book saved", "Ok");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Failure", "An error ocurred while saving the book, please try again.", "Ok");
            }
        }

        private void GetSearchResults(string query)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GoodreadsResponse));

            using (WebClient webClient = new WebClient())
            {
                var xml = Encoding.Default.GetString(webClient.DownloadData($"https://www.goodreads.com/search/index.xml?q={query}&key={Constants.GOODREADS_KEY}"));
                using (Stream reader = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var response = serializer.Deserialize(reader) as GoodreadsResponse;
                    AddBooksToBookListColleciton(response?.Search?.Results?.Work);
                }
            }
        }

        private void AddBooksToBookListColleciton(List<Work> work)
        {
            Books.Clear();

            work?.ForEach(book =>
            {
                book.Best_book.Author_Name = book.Best_book.Author.Name;
                Books.Add(book.Best_book);
            });
        }
    }
}
