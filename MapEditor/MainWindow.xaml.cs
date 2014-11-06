using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
using System.IO;
using Newtonsoft.Json.Schema;

namespace MapEditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private const int TILESIZE = 32;
        private string textureMap = "";
        private string itemMap = "";
        private string map = "";
        
        public MainWindow() {
            InitializeComponent();
        }

        private string selectFile(string type) {
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                switch(type) {
                    case "map":
                        dialog.DefaultExt = ".map";
                        dialog.Filter = "Map documents (.map)|*.map";
                        break;
                    case "texture":
                        dialog.DefaultExt = ".png";
                        dialog.Filter = "PNG (.png)|*.png";
                        break;
                }
                
                Nullable<bool> result = dialog.ShowDialog();
                if(result == true) {
                    return dialog.FileName;
                } else {
                    return "";
                }
            } catch(Exception e) {
                Console.Write(e.ToString());
                // Add error handling...
                return "";
            }
        }

        private string readFile(string filename) {
            try {
                using(StreamReader reader = new StreamReader(filename)) {
                    return reader.ReadToEnd();
                }
            } catch(Exception e) {
                Console.Write(e.ToString());
                return "Oh crap... Broken.";
                // Add error handling...
            }
        }

        private void writeFile(string filename, string content) {
            try {
                using(StreamWriter writer = new StreamWriter(filename)) {
                    writer.Write(content);
                }
            } catch(Exception e) {
                Console.Write(e.ToString());
                // Add error handling...
            }
        }

        //private void btnEncrypt_Click(object sender, RoutedEventArgs e) {
        //    string contents = readFile(this.encryptFilename);
        //    // Check if file is JSON. If not, don't try to encrypt it since it might be already.
        //    // Do better validation later: http://james.newtonking.com/json/help/index.html
        //    if(contents.ElementAt(0) == '{') {
        //        contents = Convert.ToBase64String(Encoding.UTF8.GetBytes(contents));
        //        writeFile(this.encryptFilename, contents);
        //        lblEncryptStatus.Content = "Done.";
        //        this.encryptFilename = "";
        //        //txtFilename.Text = "";
        //    }
        //}

        private Image[][] sliceImage(string image) {
            BitmapImage source = new BitmapImage(new Uri(image));
            int height = (int)(source.Height / TILESIZE);
            int width = (int)(source.Width / TILESIZE);
            Image[][] slices = new Image[height][];

            for(int y = 0; y < height; y++) {
                slices[y] = new Image[width];
                for(int x = 0; x < width; x++) {
                    Image img = new Image();
                    CroppedBitmap cropped = new CroppedBitmap(
                        (BitmapSource)source, 
                        new Int32Rect(x * TILESIZE, y * TILESIZE, TILESIZE, TILESIZE)
                    );
                    img.Source = cropped;
                    img.Stretch = Stretch.None;
                    slices[y][x] = img;
                }
            }
            return slices;
        }

        private void mnuLoadTextures_Click(object sender, RoutedEventArgs e) {
            this.textureMap = selectFile("texture");
            if(this.textureMap.Length > 0){
                Image[][] images = sliceImage(this.textureMap);

                for (int y = 0; y < images.Length; y++){
                    for (int x = 0; x < images[0].Length; x++) {
                        pnlTextures.Children.Add(images[y][x]);
                    }
                }

                pnlTextures.Height = images.Length * pnlTextures.ItemHeight;
            }
        }

        private void mnuLoadItems_Click(object sender, RoutedEventArgs e) {
            this.itemMap = selectFile("texture");
            if(this.itemMap.Length > 0) {
                Image[][] images = sliceImage(this.itemMap);

                for(int y = 0; y < images.Length; y++) {
                    for(int x = 0; x < images[0].Length; x++) {
                        pnlItems.Children.Add(images[y][x]);
                    }
                }

                pnlItems.Height = images.Length * pnlItems.ItemHeight;
            }
        }

        private void mnuLoadMap_Click(object sender, RoutedEventArgs e) {
            this.map = selectFile("map");
            if(this.map.Length > 0) {
                string json = readFile(this.map);

            }
        }
    }
}
