using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ImageEditor.Elements;
using ImageEditor.helpers;
using ImageEditor.Properties;

namespace ImageEditor.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<ImageSource> _imageList;

        public List<ImageSource> ImageList
        {
            get => _imageList;
            set
            {
                _imageList = value; 
                OnPropertyChanged(nameof(ImageList));
            }
        }

        private List<PictureData> _imageList2;
        private double _listViewWidth;
        private int _numberOfColumns;
        private PictureData _selectedPicture;
        private BitmapImage _mainImage;

        public List<PictureData> ImageList2
        {
            get => _imageList2;
            set
            {
                _imageList2 = value;
                OnPropertyChanged(nameof(ImageList2));
            }
        }



        #region Elements property

        public double ListViewWidth
        {
            get => _listViewWidth;
            set
            {
                _listViewWidth = value;
                OnPropertyChanged(nameof(ListViewWidth));
            }
        }

        #endregion



        public Canvas Canvas { get; set; }

        public ListView ListView { get; set; }

        public BitmapImage MainImage
        {
            get => _mainImage;
            set
            {
                _mainImage = value;
                OnPropertyChanged(nameof(MainImage));
                
            }
        }

        public PictureData SelectedPicture
        {
            get => _selectedPicture;
            set
            {
                _selectedPicture = value;
                OnPropertyChanged(nameof(SelectedPicture));
                SetMainImage();
            }
        }

        private void SetMainImage()
        {
            BitmapImage image = new BitmapImage(new Uri(SelectedPicture.PicturePath));
            image.Rotation = Rotation.Rotate90;
            MainImage = image;
         
        }

        public int NumberOfColumns
        {
            get => _numberOfColumns;
            set
            {
                _numberOfColumns = value;
                OnPropertyChanged(nameof(NumberOfColumns));
            }
        }

        #region SelectedElements

        #endregion

        public ZCommand DeleteElementCommand { get; set; }

        public ZCommand LoadCommand { get; set; }


        public MainViewModel(ListView listView)
        {
            ListView = listView;
            ListView.SizeChanged += ListViewSiezeChange;
            DeleteElementCommand = new ZCommand(CanDelete, Delet);
            LoadCommand = new ZCommand(CanAllow, Load);
            ImageList = new List<ImageSource>();
            ImageList2 = new List<PictureData>();
      
          LoadDefaultFolder(@"A:\fotky");
        }

        private void ListViewSiezeChange(object sender, SizeChangedEventArgs e)
        {
            double t = ListView.ActualWidth;
            if (t < 300)
            {
                NumberOfColumns = 1;
            }
            else if (t > 300 && t <= 450)
            {
                NumberOfColumns = 2;
            }
            else if (t > 450 && t <= 500)
            {
                NumberOfColumns = 3;
            }
            else
            {
                NumberOfColumns = 4;
            }
        }


        private bool CanAllow(object obj)
        {
            return true;
        }

        private void Load(object obj)
        {
            //string path = LoadFileFromDialog();
            string path = @"C:\Users\zachovaltomas\Desktop\pavel.png";
            var uri = new Uri(path);
            BitmapImage image = new BitmapImage(uri);
            ImageList.Add(image);
        }

        private void Delet(object obj)
        {
            throw new NotImplementedException();
        }


        private bool CanDelete(object obj)
        {
            return true;
        }

        private string LoadFileFromDialog()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter =
                    "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
            };

            var result = dlg.ShowDialog();

            return result == true ? dlg.FileName : null;
        }

        private void loadDefult()
        {
            string path = @"C:\Users\zachovaltomas\Desktop\pavel.png";
            var uri = new Uri(path);
            PictureData p = new PictureData{Title =  "Alfa", ImageData = new BitmapImage(uri) };
            ImageList2.Add(p);

            string path2 = @"C:\Users\zachovaltomas\Desktop\bruce.jpg";
            var uri2 = new Uri(path2);
            PictureData p2 = new PictureData { Title = "Beta", ImageData = new BitmapImage(uri2) };
            ImageList2.Add(p2);
        }

        private void LoadDefaultFolder(string path)
        {
            if(!Directory.Exists(path))
                return;

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                if (file.Contains("jpeg") || file.Contains("png") || file.Contains("jpg"))
                {
                    var buffer = System.IO.File.ReadAllBytes(file);
                    MemoryStream ms = new MemoryStream(buffer);

                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.StreamSource = ms;
                    src.DecodePixelHeight = 200;
                    src.DecodePixelWidth = 300;
                    src.EndInit();

                    var name = Path.GetFileName(file);
                    PictureData picture = new PictureData {Title = name, ImageData = src, PicturePath = file};
                    ImageList2.Add(picture);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
