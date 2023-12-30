using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace DOT.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;

        private DatabaseLoader _databaseLoader;

        public FirstViewModel(MainViewModel mvm) 
        {
            _mainViewModel = mvm;

            _databaseLoader = new DatabaseLoader();

            var Types = _databaseLoader.GetTypes();

            var ls = new List<FirstItemViewModel>();

            foreach ( var type in Types )
            {
                ls.Add(new FirstItemViewModel(type,mvm));
            }

            Buttons = new ObservableCollection<FirstItemViewModel>(ls);
        }

        private string? _searchText;

        private ObservableCollection<FirstItemViewModel> _buttons;

        public ObservableCollection<FirstItemViewModel> Buttons
        {
            get => _buttons;
            set => this.RaiseAndSetIfChanged(ref _buttons, value);
        }

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
    }
}
