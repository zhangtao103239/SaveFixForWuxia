using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace SaveFixForWuxia
{
    /// <summary>
    /// SaveFix.xaml 的交互逻辑
    /// </summary>
    public partial class SaveFix : Window
    {

        private List<dynamic> teamNpcList;
        JArray teamList;
        //converState状态码,0xa1 mode输入非法, 0xa2 查找失败 1成功
        static private int ConverState = 0; 
        //转换ID到文字,mode对应的是 1 人物,2 内功,3 天赋,4 物品
        static public String ConverID(String ID,int mode)
        {
            String filestr = null;
            try
            {
                //根据所选择的模式打开不同的文件,如果打开失败,则警告并退出.
                switch (mode)
                {
                    case 1:
                        filestr = File.ReadAllText(@"..\..\NPClist");
                        break;
                    case 2:
                        filestr = File.ReadAllText(@"..\..\neigong");
                        break;
                    case 3:
                        filestr = File.ReadAllText(@"..\..\Talent");
                        break;
                    case 4:
                        filestr = File.ReadAllText(@"..\..\itemlist");
                        break;
                    default:
                        ConverState = 0xa1;
                        return null;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("未找到程序所必需的文件,请尝试重新安装!");
                Application.Current.Shutdown();
            }
            //如果所填入的参数是ID,则正则表达式为 ID=(名字)的形式
            String pattern = ID+ @"=(.*?)(?: |。|\r\n)";
            //尝试转化为Int, 如果失败,说明填入的参数是名字,则正则表达式为 (ID)=名字 形式
            try
            {
                int Id = Convert.ToInt32(ID);
            }
            catch (FormatException)
            {
                pattern = @"([0-9]*)=" + ID;
            }
            Regex re = new Regex(pattern, RegexOptions.Multiline);
            if (re.IsMatch(filestr))
            {
                ConverState = 1;
                return re.Match(filestr).Groups[1].ToString();
            }
            else ConverState = 0xa2;
            return null;
        }
        dynamic saveJson;
        public SaveFix(ref dynamic saveJson)
        {
            this.saveJson = saveJson;
            InitializeComponent();
            this.Initial();
        }

        private void ItemListButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Initial()
        {
            this.MoneyTextBox.Text=saveJson.m_iMoney;
            this.YueLiTexTBox.Text = saveJson.m_iAttributePoints;
            teamList = saveJson.m_TeammateList;
            this.TeamListView.Items.Clear();

            teamNpcList = new List<dynamic>(teamList.Count);

            foreach (dynamic npc in saveJson.m_NpcList)
            {

                foreach(JToken teamMember in teamList)
                {
                    if (npc.iNpcID == teamMember)
                    {
                        teamNpcList.Add(npc);
                        ListViewItem teamMemberItem = new ListViewItem();
                        //Height = "30" HorizontalContentAlignment = "Center" FontSize = "15"
                        teamMemberItem.Height = 30;
                        teamMemberItem.Width = 176;
                        teamMemberItem.HorizontalAlignment = HorizontalAlignment.Center;
                        teamMemberItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                        teamMemberItem.FontSize = 15;
                        teamMemberItem.Content = ConverID(npc.iNpcID.ToString(), 1);
                        if (ConverState != 1)
                            MessageBox.Show("转换 "+npc.iNpcID.ToString()+" 时失败!\n失败代码:"+ConverState.ToString());
                        this.TeamListView.Items.Add(teamMemberItem);

                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //保存saveJson
            saveJson.m_iMoney = this.MoneyTextBox.Text;
            saveJson.m_iAttributePoints = this.YueLiTexTBox.Text;
            //打开保存对话框
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Save File|*.Save";
            sfd.InitialDirectory = Properties.Settings.Default.pathName;
            sfd.ShowDialog();
            if (String.IsNullOrEmpty(sfd.FileName))
                return;
            StreamWriter sw = new StreamWriter(sfd.FileName);
            await sw.WriteAsync(JsonConvert.SerializeObject(saveJson));
            sw.Close();
        }

        private void TeamListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int npcIndex = TeamListView.SelectedIndex;
            Console.WriteLine(ConverID(teamNpcList[npcIndex].iNpcID.ToString(), 1));
            //开启一个新的窗口用来显示用户Info
            NpcInfo npcinfo = new NpcInfo(teamNpcList[npcIndex]);
            npcinfo.Show();
        }

        private void AddTeamMate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelTeamMate_Click(object sender, RoutedEventArgs e)
        {
            
            int npcIndex = TeamListView.SelectedIndex;
            int npcID = teamNpcList[npcIndex].iNpcID.ToObject<Int32>();
            if (npcID==210001||npcID==210002||npcID==200000)
            {
                MessageBox.Show("不可以删除主角!");
                return;
            }
            teamList.RemoveAt(npcIndex);
            teamNpcList.RemoveAt(npcIndex);
            this.TeamListView.Items.RemoveAt(npcIndex);
            //teamNpcList[npcIndex].iNpcID
        }

        private void TeamMateDetail_Click(object sender, RoutedEventArgs e)
        {
            int npcIndex = TeamListView.SelectedIndex;
            Console.WriteLine(ConverID(teamNpcList[npcIndex].iNpcID.ToString(), 1));
            //开启一个新的窗口用来显示用户Info
            NpcInfo npcinfo = new NpcInfo(teamNpcList[npcIndex]);
            npcinfo.Show();
        }
    }
}
