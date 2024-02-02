using Avalonia.Controls;
using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace DOT.ViewModels
{
    public class FirstItemViewModel : ViewModelBase
    {


        public string Label => _type.Name;

        private Models.Type _type;

        private Bitmap? _cover;

        public Bitmap? Cover => _cover;

        public ReactiveCommand<Unit, Unit> Command { get; }

        public FirstItemViewModel(Models.Type type, MainViewModel mvm)
        {
            _type = type;
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.Second,type));
            try
            {
                _cover = Bitmap.DecodeToWidth(_type.LoadImage(), 400);
            }
            catch (Exception ex)
            {
                _ = Logger.Instance.Log($"Error initializing FirstItemViewModel class Error Message : {ex.Message}");
            }
        }
    }
}
