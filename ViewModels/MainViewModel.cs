using ReactiveUI;
using System.Collections.ObjectModel;
using System;

namespace DOT.ViewModels;

public class MainViewModel : ViewModelBase
{
    

    private ViewModelBase _contentViewModel;

    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    private FirstViewModel FirstView { get; set; }
    private SecondViewModel SecondView { get; set; }



    public MainViewModel()
    {
        FirstView = new FirstViewModel(this);
        //SecondView = new SecondViewModel(this);
        _contentViewModel = FirstView;
        
    }

    public enum ViewModelEnum{
        First,
        Second,
        Third,
    }

    public void ChangeViewModel(ViewModelEnum en,object? obj)
    {
        switch (en)
        {
            case ViewModelEnum.First:
                FirstView = new FirstViewModel(this);
                ContentViewModel = FirstView; 
                break;
            case ViewModelEnum.Second:
                ArgumentNullException.ThrowIfNull(obj,"obj cannot be null in here!");
                SecondView = new SecondViewModel(this, (Models.Type)obj);
                ContentViewModel = SecondView;
                break;
            case ViewModelEnum.Third:
                //ContentViewModel = new ThirdViewModel(this,(Item)obj)
                break;
        }
    }

    public void ChangeFrom3To2()
    {
        _contentViewModel = SecondView;
    }

}
