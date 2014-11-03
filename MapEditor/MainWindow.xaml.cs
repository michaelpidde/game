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
        
        public MainWindow() {
            InitializeComponent();
        }

        private string ReadFile(string filename) {
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

        private void WriteFile(string filename, string content) {
            try {
                using(StreamWriter writer = new StreamWriter(filename)) {
                    writer.Write(content);
                }
            } catch(Exception e) {
                Console.Write(e.ToString());
                // Add error handling...
            }
        }

        private void LoadTextureSheet() {
            Image textureSheet = new Image();
            BitmapImage source = new BitmapImage("")
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

    }
}
