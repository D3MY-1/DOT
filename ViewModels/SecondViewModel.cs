using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.ViewModels
{
    public class SecondViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _buttons;

        public ObservableCollection<ItemViewModel> Buttons
        {
            get => _buttons;
            set => this.RaiseAndSetIfChanged(ref _buttons, value);
        }

        private DatabaseLoader loader;

        public SecondViewModel(MainViewModel mvm,Models.Type type)
        {
            var v = type.Items;

            var ls = new List<ItemViewModel>();
            foreach (var a in v)
            {
                ls.Add(new ItemViewModel(a,mvm));
            }
            Buttons = new ObservableCollection<ItemViewModel>(ls);
        }
    }
}
