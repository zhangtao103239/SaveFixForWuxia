using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// AddSomeThing.xaml 的交互逻辑
    /// </summary>
    public partial class AddSomeThing : Window
    {
        //要返回的结果String
        public String ResultStr = null;
        //选项: 1.队员 2.内功 3.天赋 4.物品
        private int choose = 0;
        public AddSomeThing(int choose)
        {
            this.choose = choose;
            InitializeComponent();
            this.Initial();
        }
        private void Initial()
        {
            ResultStr = "";
            String filestr = null;
            try
            {
                //根据所选择的模式打开不同的文件,如果打开失败,则警告并退出.
                switch (choose)
                {
                    case 1:

                        filestr = Properties.Resources.NPClist;
                        this.NameLabel.Content = "增加队员";
                        break;
                    case 2:
                        filestr = Properties.Resources.neigong;
                        this.NameLabel.Content = "增加内功";
                        break;
                    case 3:
                        filestr = Properties.Resources.Talent;
                        this.NameLabel.Content = "增加天赋";
                        break;
                    case 4:
                        filestr = Properties.Resources.itemlist;
                        this.NameLabel.Content = "增加物品";
                        break;
                    default:
                        ResultStr = "";
                        return ;
                }

            }
            catch (IOException)
            {
                MessageBox.Show("未找到程序所必需的文件,请尝试重新安装!");
                foreach (Window win in App.Current.Windows)
                {
                   win.Close();
                }
                
            }
            String pattern =@"[0-9]*=(.*?)(?: |。|\r\n)";
            Regex re = new Regex(pattern, RegexOptions.Multiline);
            if (re.IsMatch(filestr))
            {
                this.comboBox.Items.Clear();
                int groupCount = re.Matches(filestr).Count;
                for(int groupIndex=0;groupIndex<groupCount;groupIndex++)
                {
                    ComboBoxItem myComboBoxItem = new ComboBoxItem();
                    myComboBoxItem.Width = 172.8;
                    myComboBoxItem.HorizontalAlignment = HorizontalAlignment.Left;
                    myComboBoxItem.Content = re.Matches(filestr)[groupIndex].Groups[1].ToString();
                    this.comboBox.Items.Add(myComboBoxItem);
                }
                this.comboBox.SelectedIndex = 0;
            }
            else MessageBox.Show("正则表达式匹配出错!");
            return ;
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)this.comboBox.SelectedItem;
            this.ResultStr = selectedItem.Content.ToString();
            this.Close();
        }
    }
}
