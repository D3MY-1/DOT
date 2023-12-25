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

        public string Name => _content.name;

        private Bitmap _cover;

        public Bitmap Cover => _cover;

        private int numberClicked = 0;

        //public static MainViewModel

        public ReactiveCommand<Unit, Unit> Command { get; }

        public ItemViewModel(Item item,MainViewModel mvm)
        {
            _content = item;
            _cover = Bitmap.DecodeToWidth(_content.LoadMainImage(),400);
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.First,null)); 
            
        }
    }
}
