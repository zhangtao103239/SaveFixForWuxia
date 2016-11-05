using Newtonsoft.Json.Linq;
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

namespace SaveFixForWuxia
{
    /// <summary>
    /// ChangeCout.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeCout : Window
    {
        JToken Item;
        public ChangeCout(JToken Item)
        {
            this.Item = Item;
            InitializeComponent();
            this.initial();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int ItemCountInt = 0;
            try
            {
                ItemCountInt = Convert.ToInt32(this.ItemCount.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show(this, "物品数目有误!");
                return;
            }
            if (ItemCountInt > 99 || ItemCountInt < 1)
            {
                MessageBox.Show(this, "物品数目有误!");
                return;
            }
            this.Item["m_iAmount"] = ItemCountInt;
            this.Close();
        }
        private void initial()
        {
            this.ItemName.Content = SaveFix.ConverID(this.Item["m_ItemID"].ToString(), 4) + "*";
            this.ItemCount.Text = this.Item["m_iAmount"].ToString();
        }
    }
}
