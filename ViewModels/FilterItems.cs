using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace DOT.ViewModels
{
    public class FilterItem : ReactiveObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private readonly SecondViewModel _secondViewModel;

        public FilterItem(string nme, SecondViewModel svm)
        {
            _name = nme;
            _secondViewModel = svm;
            this.WhenAnyValue(x => x.IsChecked)
                .Subscribe(StartSearch!);

        }

        private void StartSearch(bool b)
        {
            _secondViewModel.PerformSearch();
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get => _isChecked;
            set => this.RaiseAndSetIfChanged(ref _isChecked, value);
        }
    }

    public class FilterUpperItem : ViewModelBase
    {
        private string _name;

        private ObservableCollection<FilterItem> _values;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public ObservableCollection<FilterItem> Values
        {
            get => _values;
            set => this.RaiseAndSetIfChanged(ref _values, value);
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => this.RaiseAndSetIfChanged(ref _isExpanded, value);
        }


        public List<string> GetToggled()
        {
            var ret = new List<string>();
            foreach (var i in _values)
            {
                if (i.IsChecked)
                {
                    ret.Add(i.Name);
                }
            }
            return ret;
        }

        public FilterUpperItem(string Filter, List<FilterItem> values)
        {
            _name = Filter;

            _values = new ObservableCollection<FilterItem>(values);


        }
    }
}
