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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Threading;
using Newtonsoft.Json;
using System.IO;

namespace SaveFixForWuxia
{
    /// <summary>
    /// saveLoad.xaml 的交互逻辑
    /// </summary>
    public partial class saveLoad : Window
    {
        dynamic saveJson;
        String saveFileName;
        public saveLoad()
        {
            InitializeComponent();
        }

        private void SurfFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Save File|*.Save";
            ofd.InitialDirectory =Properties.Settings.Default.pathName;
            ofd.Multiselect = false;
            ofd.ShowDialog();
            this.saveFileName = ofd.FileName;
            this.PathText.Text = ofd.FileName;
            if (String.IsNullOrEmpty(this.saveFileName))
                return;
            FileInfo fi = new FileInfo(ofd.FileName);
            Properties.Settings.Default.pathName = fi.DirectoryName;
            Properties.Settings.Default.Save();
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.saveFileName))
                return;
            await Task.Run(() => this.SaveLoad());
            //Thread saveLoadThread = new Thread(new ThreadStart(this.SaveLoad));
            //saveLoadThread.Start();


            SaveFix sf = new SaveFix(ref saveJson);
            sf.Top = this.Top;
            sf.Left = this.Left;
            sf.ShowDialog();
            
        }
        private void SaveLoad()
        {
            StreamReader saveStreamReader = new StreamReader(this.saveFileName, Encoding.ASCII);
            String saveString = saveStreamReader.ReadToEnd();
            saveJson = JsonConvert.DeserializeObject(saveString);
            saveStreamReader.Close();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
