using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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
            _ = Logger.Instance.Log("Initializing FirstViewModel");
            _mainViewModel = mvm;

            _databaseLoader = new DatabaseLoader();

            

            _types = new List<FirstItemViewModel>();
            try
            {
                var Types = _databaseLoader.GetTypes();

                foreach (var type in Types)
                {
                    _types.Add(new FirstItemViewModel(type, mvm));
                }
            }
            catch (Exception ex) 
            {
                _  =Logger.Instance.Log($"Error initializing FirstViewModel class Error Message : {ex.Message}");
            }

            this.WhenAnyValue(x => x.SearchText)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(PerformSearch!);

            DisplayButtons = new ObservableCollection<FirstItemViewModel>(_types);
        }

        private string? _searchText;

        private ObservableCollection<FirstItemViewModel> _displayButtons;


        private List<FirstItemViewModel> _types;
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }


        public void PerformSearch(string s) // Need more advanced algorithm.
        {
            if (s == null) { return; }
            DisplayButtons.Clear();
            foreach (var i in _types)
            {
                if (i.Label.ToUpper().Contains(s.ToUpper()))
                {
                    DisplayButtons.Add(i);
                }
            }
            
        }

        public ObservableCollection<FirstItemViewModel> DisplayButtons
        {
            get => _displayButtons;
            set => this.RaiseAndSetIfChanged(ref _displayButtons, value);
        }

        public string? SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }
    }
}
