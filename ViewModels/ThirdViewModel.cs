using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;

namespace DOT.ViewModels
{
    public class ButtonImage : ReactiveObject, ICloneable
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

        public string? Name;

        public ButtonImage(Bitmap img)
        {
            Image = img;
        }
        public ButtonImage(Bitmap img, string name)
        {
            Image = img;
            Name = name;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }



    public class ThirdViewModel : ViewModelBase
    {
        Item _content;

        private int _selectedColorIndex = 0;

        public int SelectedColorIndex
        {
            get => _selectedColorIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedColorIndex, value);
        }

        private int _selectedImageIndex = 0;

        public int SelectedImageIndex
        {
            get => _selectedImageIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedImageIndex, value);
        }

        private int _selectedShopIndex = 0;

        public int SelectedShopIndex
        {
            get => _selectedShopIndex;
            set => this.RaiseAndSetIfChanged(ref _selectedShopIndex, value);
        }

        private int PrevImageIndex = 0;

        private int PrevShopIndex = 0;

        private int PrevColorIndex = 0;

        private ObservableCollection<ButtonImage> _shops;

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
        public ObservableCollection<ButtonImage> Shops
        {
            get => _shops;
            set => this.RaiseAndSetIfChanged(ref _shops, value);
        }


        public ReactiveCommand<Unit, Unit> Command { get; }

        private Dictionary<string, List<ButtonImage>> _col;


        public ThirdViewModel(MainViewModel mvm, Item item)
        {
            _content = item;

            this.WhenAnyValue(x => x.SelectedImageIndex)
                .Subscribe(ReportChangeImages!);

            this.WhenAnyValue(x => x.SelectedColorIndex)
                .Subscribe(ReportChangesColors!);

            this.WhenAnyValue(x => x.SelectedShopIndex)
                .Subscribe(ReportChangeShops!);

            var shop_ = new ObservableCollection<ButtonImage>();

            _col = new Dictionary<string, List<ButtonImage>>();
            foreach (var shop in item.SubItems)
            {
                if (!shop_.Any(item => item.Name == shop.ShopName))
                {
                    var shImage = DatabaseLoader.LoadImageFromAssets(shop.ShopName, "");
                    if (shImage == null)
                    {
                        _ = Logger.Instance.Log($"Error : Loading {shop} image in ThirdViewModel");
                        continue;
                    }
                    shop_.Add(new ButtonImage(Bitmap.DecodeToWidth(shImage, 800), shop.ShopName));
                }

                foreach (var color in shop.Colors)
                {
                    if (!_col.ContainsKey(color))
                    {
                        var a = item.LoadAllImages(color);

                        if (a == null)
                        {
                            _ = Logger.Instance.Log($"Error : Loading {color} images in ThirdViewModel");
                            continue;
                        }
                        var tmp = new List<ButtonImage>();
                        foreach (var image in a)
                        {
                            tmp.Add(new ButtonImage(Bitmap.DecodeToWidth(image, 800)));
                        }
                        _col[color] = tmp;
                    }
                }
            }



            Shops = new ObservableCollection<ButtonImage>(shop_);

            SelectedColorIndex = 0;
            SelectedShopIndex = 0;
            SelectedColorIndex = 0;

            ReportChangeShops(0);


            // Images = new ObservableCollection<ButtonImage>(_col[item.SubItems[SelectedShopIndex].Colors[SelectedColorIndex]]);
            MainImage = Images[0].Image;
            Images[0].BeActive();
            Colors[0].BeActive();
            Shops[0].BeActive();


            Command = ReactiveCommand.Create(() => mvm.ChangeFrom3To2());
        }

        private void ReportChangeShops(int ch)
        {
            if (Shops == null)
                return;
            var a = new ObservableCollection<ButtonImage>();
            foreach (var col in _content.SubItems[ch].Colors)
            {
                if (_col.ContainsKey(col))
                {
                    var b = (ButtonImage)_col[col][0].Clone();
                    b.Name = col;
                    a.Add(b);
                }
            }
            if (Colors != null && Colors.Count > 0)
            {
                Colors[SelectedColorIndex].Rest();
            }
            Colors = a;
            SelectedColorIndex = 0;
            if (Colors.Count > 0)
                ReportChangesColors(SelectedColorIndex);

            Shops[PrevShopIndex].Rest();
            Shops[ch].BeActive();
            PrevShopIndex = ch;
        }

        private void ReportChangesColors(int ch)
        {
            if (Colors == null || ch == -1)
                return;

            if (Colors[ch].Name != null)
            {
                if (Images != null)
                    Images[SelectedImageIndex].Rest();
                Images = new ObservableCollection<ButtonImage>(_col[Colors[ch].Name]);
            }


            SelectedImageIndex = 0;
            ReportChangeImages(SelectedImageIndex);


            Colors[PrevColorIndex].Rest();
            Colors[ch].BeActive();
            PrevColorIndex = ch;
        }

        private void ReportChangeImages(int ch)
        {
            if (Images == null || ch == -1)
                return;
            Images[PrevImageIndex].Rest();
            Images[ch].BeActive();
            MainImage = Images[ch].Image;
            PrevImageIndex = ch;
        }
    }
}
