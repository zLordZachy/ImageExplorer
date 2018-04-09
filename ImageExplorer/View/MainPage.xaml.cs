using ImageEditor.ViewModel;

namespace ImageEditor.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = new MainViewModel(ListView);
        }
    }
}
