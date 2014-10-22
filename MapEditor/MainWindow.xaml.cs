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
        private string encryptFilename = "";
        
        public MainWindow() {
            InitializeComponent();
        }

        private void openFile() {
            try {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".map";
                dialog.Filter = "Map documents (.map)|*.map";
                Nullable<bool> result = dialog.ShowDialog();
                if(result == true) {
                    this.encryptFilename = dialog.FileName;
                    txtFilename.Text = this.encryptFilename;
                    txtFilename.ScrollToEnd();
                }
            } catch(Exception e) {
                Console.Write(e.ToString());
                // Add error handling...
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

        private void btnSelectFile_Click(object sender, RoutedEventArgs e) {
            openFile();
        }

        private void txtFilename_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            if(this.encryptFilename == "") {
                openFile();
            }
        }

        private void btnEncrypt_Click(object sender, RoutedEventArgs e) {
            string contents = readFile(this.encryptFilename);
            // Check if file is JSON. If not, don't try to encrypt it since it might be already.
            // Do better validation later: http://james.newtonking.com/json/help/index.html
            if(contents.ElementAt(0) == '{') {
                contents = Convert.ToBase64String(Encoding.UTF8.GetBytes(contents));
                writeFile(this.encryptFilename, contents);
                lblEncryptStatus.Content = "Done.";
                this.encryptFilename = "";
                txtFilename.Text = "";
            }
            
        }
    }
}
