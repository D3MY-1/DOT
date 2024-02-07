using DOT.Models;
using ReactiveUI;
using System.Reactive;

namespace DOT.ViewModels
{
    public class ThirdViewModel : ViewModelBase
    {
        Item _content;

        public ReactiveCommand<Unit, Unit> Command { get; }


        public ThirdViewModel(MainViewModel mvm, Item item)
        {
            _content = item;
            Command = ReactiveCommand.Create(() => mvm.ChangeFrom3To2());
        }
    }
}
