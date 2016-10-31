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

namespace SaveFixForWuxia
{
    /// <summary>
    /// saveLoad.xaml 的交互逻辑
    /// </summary>
    public partial class saveLoad : Window
    {
        String saveFileName;

        public saveLoad()
        {
            InitializeComponent();
        }

        private void SurfFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Save File|*.Save";
            ofd.InitialDirectory = @"E:\TencentGames\rail_apps\xiakefengyunzhuanQ\Config\SaveData";
            ofd.ShowDialog();
            this.saveFileName = ofd.FileName;
            this.PathText.Text = this.saveFileName;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Thread saveLoadThread = new Thread(new ThreadStart(this.SaveLoad));
            saveLoadThread.Start();
        }
        private void SaveLoad()
        {
            
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
