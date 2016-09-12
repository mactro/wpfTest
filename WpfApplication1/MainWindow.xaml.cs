using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private BlockingCollection<BitmapImage> pictures = new BlockingCollection<BitmapImage>();
        FileSystemWatcher watcher = new FileSystemWatcher();
        public MainWindow()
        {
            InitializeComponent();
            watcher.Path = "C:/Users/maciejt/Pictures";
            watcher.Filter = "*.jpg";
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.EnableRaisingEvents = true;

            image.Source = LoadBitmapImage("C:/Users/maciejt/Pictures/1.jpg");
            new Thread(new ThreadStart(UpdateScreen)).Start();
        }
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            image.Dispatcher.Invoke(new Action(() => { image.Source = LoadBitmapImage(e.FullPath); }));
            
        }

        public static BitmapImage LoadBitmapImage(string fileName)
        {
            while (!IsFileReady(fileName))
            {
                Thread.Sleep(100);
            }
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void UpdateScreen()
        {
            while (true)
            {
                var item = pictures.Take(); // blocks if count == 0
                item.Dispatcher.Invoke(new Action(() => image.Source = item));
            }
        }

        public static bool IsFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return (inputStream.Length > 0);   
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    

}
