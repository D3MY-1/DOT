using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;

        public FirstViewModel(MainViewModel mvm)
        {
            _mainViewModel = mvm;

            var ls = new List<FirstItemViewModel>();

            var a = new List<string> { "Air_Flow"};

            ls.Add(new FirstItemViewModel("Shoes", mvm));
            ls.Add(new FirstItemViewModel("Jach", mvm));

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
