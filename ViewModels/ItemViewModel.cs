using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using DOT.Models;
using Avalonia.Media.Imaging;

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

        public ItemViewModel(Item item,MainViewModel mvm)
        {
            try
            {
                _content = item;
                _cover = Bitmap.DecodeToWidth(_content.LoadImage(), 400);
                Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.First, null));
            }
            catch (Exception ex)
            {
                Logger.Instance.Log($"Error initializing ItemViewModel class Error Message : {ex.Message}");
            }
            
            
        }
    }
}
