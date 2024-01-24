using Avalonia.Styling;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.ViewModels
{
    public class SecondViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _buttons;

        private string _searchText;

        private List<ItemViewModel> _items;

        public ReactiveCommand<Unit, Unit> Command { get; }

        public string SearchText
        {
            get => _searchText;
            set=> this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public ObservableCollection<ItemViewModel> Buttons
        {
            get => _buttons;
            set => this.RaiseAndSetIfChanged(ref _buttons, value);
        }

        

        private DatabaseLoader loader;

        public SecondViewModel(MainViewModel mvm,Models.Type type)
        {
            Logger.Instance.Log($"Initialized new SecondViewModel with this type : {type.Name}");
            var v = type.Items;

            _items = new List<ItemViewModel>();
            foreach (var a in v)
            {
                _items.Add(new ItemViewModel(a,mvm));
            }
            Buttons = new ObservableCollection<ItemViewModel>(_items);

            this.WhenAnyValue(x => x.SearchText)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(PerformSearch!);
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.First, null));
        }

        public void PerformSearch(string s) // Need more advanced algorithm.
        {
            if (s == null) { return; }
            Buttons.Clear();
            foreach (var i in _items)
            {
                if (i.Name.ToUpper().Contains(s.ToUpper()))
                {
                    Buttons.Add(i);
                }
            }

        }
    }
}
