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
        

        public string Label { get; }

        private Bitmap _cover;

        public Bitmap Cover => _cover;

        public ReactiveCommand<Unit, Unit> Command { get; }

        public FirstItemViewModel(string label, MainViewModel mvm)
        {
            Label = label;
            Command = ReactiveCommand.Create(() => mvm.ChangeViewModel(MainViewModel.ViewModelEnum.Second,Label));
        }
    }
}
