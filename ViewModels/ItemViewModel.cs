using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace DOT.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private Item _content;



        public string Name => _content.Name;

        private Bitmap _mainCover;

        private Dictionary<string, Bitmap> covers;

        public Bitmap MainCover
        {
            get => _mainCover;
            set => this.RaiseAndSetIfChanged(ref _mainCover, value);
        }

        //public static MainViewModel

        public ReactiveCommand<Unit, Unit> Command { get; }

        public List<string> GetFilterValues()
        {
            return _content.FilterValues;
        }

        public List<SubItem> GetSubitems()
        {
            return _content.SubItems;
        }

        public bool PriceInRange(float min, float max)
        {
            foreach (var item in _content.SubItems)
            {
                if (item.Price > min && item.Price < max)
                    return true;
            }
            return false;
        }

        public void ChangeColor(string color)
        {
            try
            {
                MainCover = covers[color];
            }
            catch
            {
                _ = Logger.Instance.Log("Error switching cover color!");
            }
        }

        public ItemViewModel(Item item, MainViewModel mvm)
        {
            try
            {
                covers = new Dictionary<string, Bitmap>();
                _content = item;
                var colors = new List<string>();
                foreach (var subItem in _content.SubItems)
                    foreach (var color in subItem.Colors)
                        if (!colors.Contains(color))
                        {
                            colors.Add(color);
                            try
                            {
                                covers.Add(color, Bitmap.DecodeToWidth(_content.LoadSomeImage(color), 400));
                            }
                            catch
                            {
                                _ = Logger.Instance.Log($"Error loading covers for {color} color!");
                            }
                        }

                if (covers.Count > 0)
                {
                    _mainCover = covers.ToList()[0].Value;
                }
            }
            catch (Exception ex)
            {
                _ = Logger.Instance.Log($"Error initializing ItemViewModel class Error Message : {ex.Message}");
            }

            Command = ReactiveCommand.Create(() => { mvm.ChangeViewModel(MainViewModel.ViewModelEnum.Third, item); });
        }
    }
}
