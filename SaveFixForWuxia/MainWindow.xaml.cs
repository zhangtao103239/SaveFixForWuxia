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

namespace SaveFixForWuxia
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window win in App.Current.Windows)
            {
                win.Close();
            }
        }

        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            saveLoad mySaveload = new saveLoad();
            mySaveload.Top = this.Top;
            mySaveload.Left = this.Left + (this.Width/3);
            mySaveload.ShowDialog();
        }
    }
}
