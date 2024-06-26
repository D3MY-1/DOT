﻿using Avalonia.Media.Imaging;
using DOT.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private ObservableCollection<string> _infoText;

        public ObservableCollection<string> InfoText
        {
            get => _infoText;
            set => this.RaiseAndSetIfChanged(ref _infoText, value);
        }

        public ReactiveCommand<Unit, Unit> Command { get; }

        private Dictionary<string, List<ButtonImage>> _col;


        private List<string> infConst;



        enum ChListNames
        {
            Shop,
            Color,
            Price
        }

        private List<string> _infChang;


        public ThirdViewModel(MainViewModel mvm, Item item, List<string> FilterNames)
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


            infConst = new List<string>();
            _infChang = new List<string>();

            if (item.FilterValues.Count == FilterNames.Count)

                for (int i = 0; i < item.FilterValues.Count; i++)
                {
                    infConst.Add($"{FilterNames[i]} - {item.FilterValues[i]}");
                }
            infConst.Add("");
            infConst.Add("");
            infConst.Add("");
            _infChang.Add($"Shop - {item.SubItems[SelectedShopIndex].ShopName}");
            _infChang.Add($"Color - {item.SubItems[SelectedShopIndex].Colors[SelectedColorIndex]}");
            _infChang.Add($"Price - €{item.SubItems[SelectedShopIndex].Price}");


            UpdateInfo();
            Command = ReactiveCommand.Create(() => mvm.ChangeFrom3To2());
        }

        private void UpdateInfo()
        {
            InfoText = new ObservableCollection<string>(infConst.Concat(_infChang));
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
                    a.Add(new ButtonImage(_col[col][0].Image, col));
                }
            }
            if (Colors != null && Colors.Count > 0)
            {
                Colors[SelectedColorIndex].Rest();
            }
            if (_infChang != null)
            {
                _infChang[(int)ChListNames.Shop] = $"{ChListNames.Shop.ToString()} - {_content.SubItems[ch].ShopName}";
                _infChang[(int)ChListNames.Price] = $"Price - €{_content.SubItems[ch].Price}";
                UpdateInfo();
            }

            Colors = a;
            SelectedColorIndex = 0;
            SelectedImageIndex = 0;
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

            if (_infChang != null)
            {
                _infChang[(int)ChListNames.Color] = $"{ChListNames.Color.ToString()} - {Colors[ch].Name}";
                UpdateInfo();
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
