using ImageEditor.Elements;
using ImageEditor.helpers;
using ImageEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageExplorer.ViewModel
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


        private readonly Image _image;
        private readonly Border _border;

        private Point _origin;
        private Point _start;
        public Transform BaseTransform { get; set; }
        public ScaleTransform Xform  { get; set; }


        public TranslateTransform Tt { get; set; }

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

        private readonly Page _page;

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
                RightTurnCommand.ChangeCanExecute();
                LeftTurnCommand.ChangeCanExecute();
            }
        }

        private void SetMainImage()
        {
            BitmapImage image = new BitmapImage(new Uri(SelectedPicture.PicturePath));
            image.Rotation = Rotation.Rotate90;
            MainImage = image;
            _page.WindowTitle = SelectedPicture.Title;
            ResetZoom();
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

        public ZCommand RightTurnCommand { get; set; }
        public ZCommand LeftTurnCommand { get; set; }
        public ZCommand Turn270Command { get; set; }
        public ZCommand InfoCommand { get; set; }



        public MainViewModel(ListView listView, Page page, Image image, Border border)
        {
            ListView = listView;
            _page = page;
            _image = image;
            _border = border;
            ListView.SizeChanged += ListViewSiezeChange;
            DeleteElementCommand = new ZCommand(CanDelete, Delet);
            LoadCommand = new ZCommand(CanAllow, Load);
            RightTurnCommand = new ZCommand(CanTurn, Trun180);
            LeftTurnCommand = new ZCommand(CanTurn, Trun90);
            Turn270Command = new ZCommand(CanTurn, Trun270);
            InfoCommand = new ZCommand(CanTurn, Info);
            ImageList = new List<ImageSource>();
            ImageList2 = new List<PictureData>();

            InitRanderTransformation();
      
          LoadDefaultFolder(@"A:\fotky");
        }

        private void Info(object obj)
        {
            var x = MainImage.DpiX;
            var y = MainImage.DpiY;

         //   MainImage.


        }

        private void Trun90(object obj)
        {
            var buffer = File.ReadAllBytes(SelectedPicture.PicturePath);
            MemoryStream ms = new MemoryStream(buffer);

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.Rotation = Rotation.Rotate90;
            src.EndInit();

            MainImage = src;
        }

        private bool CanTurn(object obj)
        {
            return MainImage != null;
        }

        private void Trun180(object obj)
        {
            var buffer = File.ReadAllBytes(SelectedPicture.PicturePath);
            MemoryStream ms = new MemoryStream(buffer);

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.Rotation = Rotation.Rotate180;
            src.EndInit();

            MainImage = src;
        }

        private void Trun270(object obj)
        {
            var buffer = File.ReadAllBytes(SelectedPicture.PicturePath);
            MemoryStream ms = new MemoryStream(buffer);

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.Rotation = Rotation.Rotate270;
            src.EndInit();

            MainImage = src;
        }

        private void InitRanderTransformation()
        {
            TransformGroup group = new TransformGroup();

            ScaleTransform xform = new ScaleTransform();
            group.Children.Add(xform);
            Xform = xform;
            TranslateTransform tt = new TranslateTransform();
            Tt = tt;
            group.Children.Add(tt);

            _image.RenderTransform = group;
            _image.MouseWheel += image_MouseWheel;
            _image.MouseLeftButtonDown += image_MouseLeftButtonDown;
            _image.MouseLeftButtonUp += image_MouseLeftButtonUp;
            _image.MouseMove += image_MouseMove; 
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

        private void LoadDefult()
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
                    var buffer = File.ReadAllBytes(file);
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

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _image.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_image.IsMouseCaptured) return;

            var tt = (TranslateTransform)((TransformGroup)_image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            Vector v = _start - e.GetPosition(_border);
            tt.X = _origin.X - v.X;
            tt.Y = _origin.Y - v.Y;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                ResetZoom();
                return;
            }
            _image.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)_image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            _start = e.GetPosition(_border);
            _origin = new Point(tt.X, tt.Y);
        }

        private void ResetZoom()
        {
            var ttt = (TranslateTransform)((TransformGroup)_image.RenderTransform).Children.First(tr => tr is TranslateTransform);
            ttt.X = 0;
            ttt.Y = 0;

            TransformGroup transformGroup = (TransformGroup)_image.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

            transform.ScaleX = 1;
            transform.ScaleY = 1;
            return;
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TransformGroup transformGroup = (TransformGroup)_image.RenderTransform;
            ScaleTransform transform = (ScaleTransform)transformGroup.Children[0];

            double zoom = e.Delta > 0 ? .2 : -.2;
            transform.ScaleX += zoom;
            transform.ScaleY += zoom;
        }
    

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
