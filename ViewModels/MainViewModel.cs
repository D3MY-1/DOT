using ReactiveUI;
using System.Collections.ObjectModel;

namespace DOT.ViewModels;

public class MainViewModel : ViewModelBase
{
    

    private ViewModelBase _contentViewModel;

    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public FirstViewModel FirstView { get; }
    public SecondViewModel SecondView { get; }



    public MainViewModel()
    {
        FirstView = new FirstViewModel(this);
        SecondView = new SecondViewModel(this);
        _contentViewModel = SecondView;
    }

}
