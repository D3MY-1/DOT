using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace DOT.ViewModels
{

    public class StringNumericComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (int.TryParse(x, out int xNum) && int.TryParse(y, out int yNum))
            {
                return xNum.CompareTo(yNum);
            }
            else
            {
                return string.Compare(x, y);
            }
        }
    }

    public class SecondViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _buttons;


        private string _searchText;

        private List<ItemViewModel> _items;

        private ObservableCollection<FilterUpperItem> _filterItems;


        private ObservableCollection<FilterUpperItem> _subFilterItems;

        private float maxPrice = float.MinValue;

        private float minPrice = float.MaxValue;

        private float _lowerSelectedValue = 0;

        private float _upperSelectedValue = 100;

        private int _lowPrice;

        private int _highPrice;

        public ReactiveCommand<Unit, Unit> Command { get; }


        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
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

        public ObservableCollection<FilterUpperItem> SubFilterItems
        {
            get => _subFilterItems;
            set => this.RaiseAndSetIfChanged(ref _subFilterItems, value);
        }

        public float LowerSelectedValue
        {
            get => _lowerSelectedValue;
            set => this.RaiseAndSetIfChanged(ref _lowerSelectedValue, value);
        }

        public float UpperSelectedValue
        {
            get => _upperSelectedValue;
            set => this.RaiseAndSetIfChanged(ref _upperSelectedValue, value);
        }

        public int LowPrice
        {
            get => _lowPrice;
            set => this.RaiseAndSetIfChanged(ref _lowPrice, value);
        }

        public int HighPrice
        {
            get => _highPrice;
            set => this.RaiseAndSetIfChanged(ref _highPrice, value);
        }

        private static string transform(string text)
        {
            float n;
            if (float.TryParse(text.Replace(",", "."), out n))
                return Math.Floor(n).ToString();
            return text;
        }


        public SecondViewModel(MainViewModel mvm, Models.Type type) // Very bad system right now. Spaghetti mess.
        {

            _ = Logger.Instance.Log($"Initialized new SecondViewModel with this type : {type.Name}");
            var v = type.Items;


            //var filterValue = new Dictionary<string, HashSet<string>>();
            var filterValue = new List<HashSet<string>>();

            foreach (var filter in type.Filters)
            {
                filterValue.Add(new HashSet<string>());   //This needs to be remade
            }


            _items = new List<ItemViewModel>();         // Very bad code //
            var colItems = new List<FilterItem>();
            var sizeItems = new List<FilterItem>();
            var priceItems = new List<FilterItem>();
            var colors = new HashSet<string>();
            var sizes = new List<string>();

            foreach (var a in v)
            {
                foreach (var sub in a.SubItems) // Sub filters
                {
                    foreach (var color in sub.Colors)
                    {
                        colors.Add(color);
                    }
                    foreach (var size in sub.Sizes)
                    {
                        if (!sizes.Contains(transform(size)))
                            sizes.Add(transform(size));
                    }

                    if (sub.Price < minPrice)
                        minPrice = sub.Price;
                    else if (sub.Price > maxPrice)
                        maxPrice = sub.Price;
                }

                _items.Add(new ItemViewModel(a, mvm)); //Main Filters
                for (var i = 0; i < type.Filters.Count; i++)
                {
                    filterValue[i].Add(a.FilterValues[i]); // Too complicated
                }
            }

            sizes.Sort(new StringNumericComparer());

            foreach (var a in colors)
            {
                colItems.Add(new FilterItem(a, this));
            }
            foreach (var a in sizes)
                sizeItems.Add(new FilterItem(a, this));



            SubFilterItems = new ObservableCollection<FilterUpperItem>();


            SubFilterItems.Add(new FilterUpperItem("Color", colItems));
            SubFilterItems.Add(new FilterUpperItem("Size", sizeItems));


            _filterItems = new ObservableCollection<FilterUpperItem>();

            for (var i = 0; i < filterValue.Count; i++)
            {
                var items = new List<FilterItem>();
                foreach (var a in filterValue[i])
                    items.Add(new FilterItem(a, this));
                _filterItems.Add(new FilterUpperItem(type.Filters[i], items));
            }






            Buttons = new ObservableCollection<ItemViewModel>(_items);

            this.WhenAnyValue(x => x.LowerSelectedValue)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RecalculatePrice!);

            this.WhenAnyValue(x => x.UpperSelectedValue)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(RecalculatePrice!);



            this.WhenAnyValue(x => x.SearchText)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Throttle(TimeSpan.FromMilliseconds(50))
                .Subscribe(StartSearch!);
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.First, null));
        }

        private void StartSearch(string s)
        {
            PerformSearch();
        }
        private void StartSearch(int s)
        {
            PerformSearch();
        }
        private readonly object searchLock = new object();

        public void PerformSearch() // Need more advanced algorithm.
        {

            if (Buttons == null)
                return;
            List<ItemViewModel> newItems = new List<ItemViewModel>();
            var s = SearchText;
            //Buttons.Clear();
            var col = SubFilterItems[0].GetToggled();
            var sizes = SubFilterItems[1].GetToggled();

            bool performFilterSearch = false;
            bool performSubFilterSearch = col.Count > 0 || sizes.Count > 0;
            foreach (var item in _filterItems)
            {
                if (item.GetToggled().Count > 0) { performFilterSearch = true; break; }
            }




            foreach (var i in _items)
            {
                bool isValid = true;
                if (performFilterSearch)
                {
                    isValid = PerformFilterCheck(i);
                }
                if (performSubFilterSearch && isValid)
                {
                    isValid = PerformSubFilterCheck(i);
                }
                if (isValid && IfSimilarADD(i, s))
                    newItems.Add(i);
                //IfSimilarADD(i, s);
            }
            Buttons = new ObservableCollection<ItemViewModel>(newItems);
        }


        private bool PerformFilterCheck(ItemViewModel it)
        {
            var values = it.GetFilterValues();

            for (var j = 0; j < values.Count; j++)
            {
                var v = FilterItems[j].GetToggled();
                if ((v.Count == 0) || (v.Count > 0 && v.Contains(values[j])))
                    continue;

                return false;
            }
            return true;
        }

        private bool PerformSubFilterCheck(ItemViewModel it)
        {
            var col = SubFilterItems[0].GetToggled();
            var sizes = SubFilterItems[1].GetToggled();
            foreach (var v in it.GetSubitems())
            {
                var inter = v.Colors.Intersect(col);
                if (col.Count > 0 && inter.Count() <= 0)
                {
                    continue;
                }
                var h = new List<string>();
                foreach (var indexer in v.Sizes)
                    h.Add(transform(indexer));


                if (sizes.Count > 0 && !Helper.SharesAnyValueWith(h, sizes))
                {
                    continue;
                }
                if (inter.Count() > 0)
                    it.ChangeColor(inter.First());
                return true;
            }
            return false;
        }


        private void RecalculatePrice(float val)
        {
            var v = (int)Math.Round((maxPrice - minPrice) * (val / 100) + minPrice);

            if (val.Equals(LowerSelectedValue))
                LowPrice = v;
            if (val.Equals(UpperSelectedValue))
                HighPrice = v;
            PerformSearch();

        }

        private bool IfSimilarADD(ItemViewModel item, string s)
        {
            if (!item.PriceInRange(LowPrice, HighPrice))
                return false;
            //if (s == null)
            //{
            //Buttons.Add(item);
            //    Logger.Instance.Log("Add Item!!!!!!!!");
            //   return true;
            //}
            //else if (item.Name.ToUpper().Contains(s.ToUpper()))
            //Buttons.Add(item);
            //   return true;
            return s == null || item.Name.ToUpper().Contains(s.ToUpper());
        }


    }

}
