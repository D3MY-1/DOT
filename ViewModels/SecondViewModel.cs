﻿using Avalonia.Styling;
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
using Avalonia.Controls;
using Avalonia.Dialogs.Internal;
using System.Runtime.InteropServices;
using DynamicData;

namespace DOT.ViewModels
{
    public class SecondViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _buttons;


        private string _searchText;

        private List<ItemViewModel> _items;

        private ObservableCollection<FilterUpperItem> _filterItems;

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

        public ObservableCollection<FilterUpperItem> FilterItems
        {
            get => _filterItems;
            set => this.RaiseAndSetIfChanged(ref _filterItems, value);
        }

        

        private DatabaseLoader loader;

        

        public SecondViewModel(MainViewModel mvm,Models.Type type)
        {
            _ = Logger.Instance.Log($"Initialized new SecondViewModel with this type : {type.Name}");
            var v = type.Items;


            //var filterValue = new Dictionary<string, HashSet<string>>();
             var filterValue = new List<HashSet<string>>();

            foreach(var filter in type.Filters)
            {
                filterValue.Add(new HashSet<string>());   //This needs to be remade
            }

            

            _items = new List<ItemViewModel>();
            foreach (var a in v)
            {
                _items.Add(new ItemViewModel(a,mvm));
                for (var i = 0;i < type.Filters.Count;i++) 
                {
                    filterValue[i].Add(a.FilterValues[i]); // Too complicated
                }
            }

            _filterItems = new ObservableCollection<FilterUpperItem>();

            for (var i = 0;i < filterValue.Count;i++)
            {
                var items = new List<FilterItem>();
                foreach (var a in filterValue[i])
                    items.Add(new FilterItem(a,this));
                _filterItems.Add(new FilterUpperItem(type.Filters[i], items));
            }


            
            
            
            
            Buttons = new ObservableCollection<ItemViewModel>(_items);

            this.WhenAnyValue(x => x.SearchText)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(StartSearch!);
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.First, null));
        }

        private void StartSearch(string s)
        {
            PerformSearch();
        }

        public void PerformSearch() // Need more advanced algorithm.
        {
            if(Buttons == null)
                return;
            var s = SearchText;

            Buttons.Clear();

            bool performFilterSearch = false;
            foreach (var item in _filterItems) 
            {
                if(item.GetToggled().Count > 0) {  performFilterSearch = true; break; }
            }

            if (!performFilterSearch && s == null)
            {
                Buttons.Add(_items); 
                return;
            }
            

            

            foreach (var i in _items)
            {
                if(performFilterSearch)
                {
                    var values = i.GetFilterValues();
                    bool isValid = true;
                    for (var j = 0; j < values.Count; j++)
                    {
                        var v = FilterItems[j].GetToggled();
                        if ((v.Count == 0) || (v.Count > 0 && v.Contains(values[j])))
                            continue;
                        
                        isValid = false;
                        break;
                    }
                    if (isValid)
                        IfSimilarADD(i, s);
                }
                else
                    IfSimilarADD(i, s);
            }

        }


        private void IfSimilarADD(ItemViewModel item, string s)
        {
            if (s == null)
            {
                Buttons.Add(item);
                return;
            }
            
            if (item.Name.ToUpper().Contains(s.ToUpper()))
                Buttons.Add(item);
        }
    }

}
