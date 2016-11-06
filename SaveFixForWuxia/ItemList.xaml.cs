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
        private dynamic saveJson;
        private Button ParentItemButton;
        public ItemList(dynamic saveJson,Button ParentItemButton)
        {
            this.saveJson = saveJson;
            this.ItemJarray = new JArray(saveJson.m_BackpackList);
            this.ParentItemButton = ParentItemButton;
            InitializeComponent();
            this.Initial();
        }

        private void Initial()
        {
            this.ItemListView.Items.Clear();
            foreach(JToken item in this.ItemJarray)
            {
                ListViewItem myItem = new ListViewItem();
                myItem.Height = 35;
                myItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                myItem.Content = SaveFix.ConverID(item["m_ItemID"].ToString(), 4)+"*"+ item["m_iAmount"].ToString();
                this.ItemListView.Items.Add(myItem);
            }
            //< ListViewItem  Height = "35" HorizontalContentAlignment = "Center" Content = "大宝剑:1" />
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.saveJson.m_BackpackList = this.ItemJarray;
            this.Close();
        }

        private void BcakButton_Click(object sender, RoutedEventArgs e)
        {
            //此处声明 放弃所有修改
            if (MessageBox.Show("放弃本次更改并返回吗?", "取消", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else return;
        
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.ParentItemButton.IsEnabled = true;
        }

        private void DelItemButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectIndex = this.ItemListView.SelectedIndex;
            this.ItemJarray.RemoveAt(SelectIndex);
            Initial();
        }

        private void ChangeCountButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectIndex = this.ItemListView.SelectedIndex;
            if (SelectIndex == -1)
                return;
            ChangeCout newChangeCout = new ChangeCout(this.ItemJarray[SelectIndex]);
            newChangeCout.Top = this.Top + this.Height / 3;
            newChangeCout.Left = this.Left + this.Width / 3;
            newChangeCout.ShowDialog();
            this.Initial();
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            AddSomeThing addTeammate = new AddSomeThing(4);
            addTeammate.Top = this.Top + this.Height / 3;
            addTeammate.Left = this.Left + this.Width / 3;
            addTeammate.ShowDialog();
            if (String.IsNullOrEmpty(addTeammate.ResultStr))
                return;
            String ItemID = SaveFix.ConverID(addTeammate.ResultStr, 4);
            foreach (JToken item in ItemJarray)
            {
                if (item["m_ItemID"].ToString() == ItemID)
                {
                    MessageBox.Show("该物品已经存在!", "错误");
                    return;
                }
            }

            JToken newItem = new JObject();
            newItem["m_iAmount"] = "1";
            newItem["m_ItemID"] = ItemID;
            newItem["m_bNew"] = "True";
            ItemJarray.Add(newItem);
            this.Initial();
        }
    }                                                                                                   
}
