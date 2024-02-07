using ReactiveUI;
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
        _ = Logger.Instance.Log("Data context is equal FirstView");
        //SecondView = new SecondViewModel(this);
        _contentViewModel = FirstView;

    }

    public enum ViewModelEnum
    {
        First,
        Second,
        Third,
    }

    public void ChangeViewModel(ViewModelEnum en, object? obj)
    {
        _ = Logger.Instance.Log($"ContentViewModel is changed to {en.ToString()} ViewModel");
        try
        {
            switch (en)
            {
                case ViewModelEnum.First:
                    FirstView = new FirstViewModel(this);
                    ContentViewModel = FirstView;
                    break;
                case ViewModelEnum.Second:
                    ArgumentNullException.ThrowIfNull(obj, "obj cannot be null in here!");
                    SecondView = new SecondViewModel(this, (Models.Type)obj);
                    ContentViewModel = SecondView;
                    break;
                case ViewModelEnum.Third:
                    ArgumentNullException.ThrowIfNull(obj, "obj cannot be null in here!");
                    ContentViewModel = new ThirdViewModel(this, (Models.Item)obj);
                    break;
            }
        }
        catch (Exception ex)
        {
            _ = Logger.Instance.Log($"Error Message : {ex.Message}");
        }

    }

    public void ChangeFrom3To2()
    {
        _ = Logger.Instance.Log($"ContentViewModel is changed from Third ViewModel to Second ViewModel");
        ContentViewModel = SecondView;
    }

}
