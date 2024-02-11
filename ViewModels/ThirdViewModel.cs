using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace DOT.ViewModels
{
    public class ButtonImage : ReactiveObject
    {
        private string _borderColor = "LightGray";

        public string BorderColor
        {
            get => _borderColor;
            set => this.RaiseAndSetIfChanged(ref _borderColor, value);
        }

        private Bitmap _image;

        public Bitmap Image
        {
            get => _image;
            set => this.RaiseAndSetIfChanged(ref _image, value);
        }

        public void BeActive()
        {
            BorderColor = "Black";
        }

        public void Rest()
        {
            BorderColor = "LightGray";
        }


        public ButtonImage(Bitmap img)
        {
            Image = img;
        }
    }

    public class ThirdViewModel : ViewModelBase
    {
        Item _content;

        string _selectedColor;

        private int _selectedImageIndex = 0;

        public int SelectedImageIndex
        {
            get => _selectedImageIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedImageIndex, value);
        }

        private int PrevImageIndex = -1;

        private ObservableCollection<ButtonImage> _images;

        private ObservableCollection<ButtonImage> _colors;

        private Bitmap _mainimage;
        public Bitmap MainImage
        {
            get => _mainimage;
            set => this.RaiseAndSetIfChanged(ref _mainimage, value);
        }

        public ObservableCollection<ButtonImage> Colors
        {
            get => _colors;
            set => this.RaiseAndSetIfChanged(ref _colors, value);
        }
        public ObservableCollection<ButtonImage> Images
        {
            get => _images;
            set => this.RaiseAndSetIfChanged(ref _images, value);
        }

        public ReactiveCommand<Unit, Unit> Command { get; }


        public ThirdViewModel(MainViewModel mvm, Item item)
        {

            this.WhenAnyValue(x => x.SelectedImageIndex)
                .Subscribe(ReportChange!);

            _selectedColor = "White";

            var res = item.LoadAllImages(_selectedColor);

            if (res == null)
            {
                _ = Logger.Instance.Log("Error : Loading images in ThirdViewModel");
            }

            Images = new ObservableCollection<ButtonImage>();
            foreach (var i in res)
                Images.Add(new ButtonImage(Bitmap.DecodeToWidth(i, 800)));
            MainImage = Images[0].Image;
            Images[0].BeActive();

            _content = item;
            Command = ReactiveCommand.Create(() => mvm.ChangeFrom3To2());
        }

        private void ReportChange(int ch)
        {
            if (Images == null)
                return;
            if (PrevImageIndex != -1)
                Images[PrevImageIndex].Rest();
            Images[ch].BeActive();
            MainImage = Images[ch].Image;
            PrevImageIndex = ch;
        }
    }
}
