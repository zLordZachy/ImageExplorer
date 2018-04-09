using System.Windows.Controls;
using VektorovyEditor.ViewModel;

namespace VektorovyEditor.View
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
