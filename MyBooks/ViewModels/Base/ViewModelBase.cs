using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyBooks.ViewModels.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public ViewModelBase()
        {
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
