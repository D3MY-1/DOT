using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.ViewModels
{
    public class FirstViewModel : ViewModelBase
    {
        private MainViewModel _mainViewModel;

        public FirstViewModel(MainViewModel model)
        {
            _mainViewModel = model;
        }

        private string? _searchText;


        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
    }
}
