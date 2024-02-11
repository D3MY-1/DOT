using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace DOT.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private Item _content;

        public string Name => _content.Name;

        private Bitmap _cover;

        public Bitmap Cover => _cover;

        //public static MainViewModel

        public ReactiveCommand<Unit, Unit> Command { get; }

        public List<string> GetFilterValues()
        {
            return _content.FilterValues;
        }

        public ItemViewModel(Item item, MainViewModel mvm)
        {
            try
            {
                _content = item;
                _cover = Bitmap.DecodeToWidth(_content.LoadSomeImage(), 400);
            }
            catch (Exception ex)
            {
                _ = Logger.Instance.Log($"Error initializing ItemViewModel class Error Message : {ex.Message}");
            }

            Command = ReactiveCommand.Create(() => { mvm.ChangeViewModel(MainViewModel.ViewModelEnum.Third, item); });
        }
    }
}
