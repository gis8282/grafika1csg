using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Csg.Gui.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Microsoft.Windows.Controls.Ribbon.RibbonWindow
    {
        private WriteableBitmap _bitmap;
        private int[] _buffer = new int[2000 * 2000];
        private RayCaster _rayCaster;

        private int ImageWidth { get; set; }
        private int ImageHeight { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeRayCaster();
            InitializeImage();
        }

        private void InitializeRayCaster()
        {

            _rayCaster = new RayCaster(PutPixel, (x, y, width, height) => { });
            var sceneParser = new SphereScriptParser();
            var lightsParser = new TextLightsParser();
            _rayCaster.Root = sceneParser.ParseScene(@"c:\Users\Adam\Desktop\grafika1csg\scene and lights\puchar.sl");
            _rayCaster.Lights = lightsParser.ParseLights(@"c:\Users\Adam\Desktop\grafika1csg\scene and lights\light front.txt");
        }

        private void InitializeImage()
        {
            ImageWidth = ImageHeight = 1;
            _bitmap = new WriteableBitmap(1, 1, 96, 96, PixelFormats.Bgr32, null);
            _bitmap.WritePixels(new Int32Rect(0, 0, 1, 1), _buffer, 4, 0);
            MainWindowImage.Source = _bitmap;
        }

        private void MainWindowCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ImageWidth = (int)e.NewSize.Width;
            ImageHeight = (int)e.NewSize.Height;

            _rayCaster.Width = ImageWidth;
            _rayCaster.Height = ImageHeight;
            _bitmap = new WriteableBitmap(ImageWidth, ImageHeight, 96, 96, PixelFormats.Bgr32, null);

            Render(); 
        }

        protected void Render()
        {
            Array.Clear(_buffer, 0, ImageWidth * ImageHeight);
            _rayCaster.RayCast();

            _bitmap.WritePixels(new Int32Rect(0, 0, ImageWidth, ImageHeight), _buffer, 4 * ImageWidth, 0);
            MainWindowImage.Source = _bitmap;
        }

        public void PutPixel(int x, int y, int r, int g, int b)
        {
            var width = ImageWidth;
            _buffer[y * width + x] = (r << 16) | (g << 8) | b;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
