using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AForge.Video;
using System.Threading;
using System.Configuration;
using Microsoft.Windows.Controls;

namespace LegoMindstormsController
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Timer to check controller actions
        /// </summary>
        DispatcherTimer controllerTimer = new DispatcherTimer();
        NXTController nc = new NXTController();
        public MJPEGStream stream;
        private Object lockObj = new Object();
        private Bitmap currentImage;
        private static int fps = 0;

        public MainWindow()
        {
            InitializeComponent();
            controllerTimer = new DispatcherTimer();
            controllerTimer.Tick += new EventHandler(controllerTimer_Tick);
            controllerTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            controllerTimer.Start();
            lblInfo.Content = nc.Connect();

            // create MJPEG video source
            stream = new MJPEGStream(ConfigurationManager.AppSettings["CameraStreamUrl"].ToString());
            // set event handlers
            stream.NewFrame += new NewFrameEventHandler(videoNewImage);
            // start the video source
            stream.Start();
        }

        private void videoNewImage(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();

            lock (lockObj)
            {
                if (currentImage != null)
                    currentImage.Dispose();
                currentImage = (Bitmap)eventArgs.Frame.Clone();
            }

            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (SendOrPostCallback)delegate
            {
                var hBitmap = img.GetHbitmap();

                System.Windows.Media.Imaging.BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                imgCam.Source = bitmapSource;
                fps++;

                NativeMethods.DeleteObject(hBitmap);

            }, null);

            img.Dispose();
            img = null;
            GC.Collect();
        }

        private void getImage(object source, ElapsedEventArgs e)
        {
            if (currentImage == null)
                return;
            lock (lockObj)
            {
                currentImage.Save("tmp.bmp", ImageFormat.Bmp);
            }
        }

        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

       
    }
}
