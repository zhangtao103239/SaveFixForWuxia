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
using Newtonsoft.Json.Linq;

namespace SaveFixForWuxia
{
    /// <summary>
    /// ItemList.xaml 的交互逻辑
    /// </summary>
    public partial class ItemList : Window
    {
        private JArray ItemJarray;
        private Button ParentItemButton;
        public ItemList(JArray ItemJarray,Button ParentItemButton)
        {
            this.ItemJarray = ItemJarray;
            this.ParentItemButton = ParentItemButton;
            InitializeComponent();
        }

        private void Initial()
        {
            foreach(JValue item in this.ItemJarray)
            {
                ListViewItem myItem = new ListViewItem();
                myItem.Height = 35;
                myItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                Content = "";
                this.ItemListView.Items.Add(myItem);
            }
            //< ListViewItem  Height = "35" HorizontalContentAlignment = "Center" Content = "大宝剑:1" />
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BcakButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.ParentItemButton.IsEnabled = true;
        }
    }
}
