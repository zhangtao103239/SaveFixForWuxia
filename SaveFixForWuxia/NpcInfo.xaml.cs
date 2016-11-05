using Newtonsoft.Json.Linq;
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
    /// NpcInfo.xaml 的交互逻辑
    /// </summary>
    public partial class NpcInfo : Window
    {
        dynamic npc;
        JArray NeigongJArray;
        JArray TalentJArray;
        public NpcInfo(dynamic npc)
        {
            this.npc = npc;
            NeigongJArray = new JArray(this.npc.NeigongList);
            TalentJArray = new JArray(this.npc.TalentList);
            InitializeComponent();
            this.Initial();
        }
        private void Initial()
        {
            this.NameLabel.Content = SaveFix.ConverID(this.npc.iNpcID.ToString(), 1);
            this.NeigongListView.Items.Clear();
            foreach(dynamic NeiGong in NeigongJArray)
            {
                ListViewItem neigong = new ListViewItem();
                //Height = "35" Width = "163"
                neigong.Width = 163;
                neigong.Height = 35;
                neigong.HorizontalContentAlignment = HorizontalAlignment.Center;
                String neigongStr = SaveFix.ConverID(NeiGong.iSkillID.ToString(), 2);
                String neigongLevel = NeiGong.iLevel.ToString();
                neigong.Content = neigongStr + ":  Lv" + neigongLevel;
                this.NeigongListView.Items.Add(neigong);
            }
            this.TalentListView.Items.Clear();
            foreach(JValue talentID in TalentJArray)
            {
                ListViewItem talentItem = new ListViewItem();
                talentItem.Width = 163;
                talentItem.Height = 31;
                talentItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                talentItem.Content=SaveFix.ConverID(talentID.ToString(), 3);
                String TalentfileStr = File.ReadAllText(@"..\..\Talent");
                String pattenStr = talentItem.Content.ToString() + @"。(.*?)\r\n";
                Regex re = new Regex(pattenStr,RegexOptions.Multiline);
                if(re.IsMatch(TalentfileStr))
                {
                    talentItem.ToolTip= re.Match(TalentfileStr).Groups[1];
                }
                else
                {
                    MessageBox.Show("正则表达式出错啦!请联系开发者");
                }
                this.TalentListView.Items.Add(talentItem);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("放弃本次更改并返回吗?", "取消", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else return;
        }

        private void DelNeiGongButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectIndex = this.NeigongListView.SelectedIndex;
            if (SelectIndex == -1)
                return;
            this.NeigongJArray.RemoveAt(SelectIndex);
            this.NeigongListView.Items.RemoveAt(SelectIndex);
            //this.npc.NeigongList=this.NeigongJArray;
        }

        private void ChangeLevelButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectIndex = this.NeigongListView.SelectedIndex;
            if (SelectIndex == -1)
                return;
            ChangeLevel newChangelevel = new ChangeLevel(this.NeigongJArray[SelectIndex]);
            newChangelevel.Top = this.Top + this.Height / 3;
            newChangelevel.Left = this.Left + this.Width / 3;
            newChangelevel.ShowDialog();
            this.Initial();
        }

        private void DelTalentButton_Click(object sender, RoutedEventArgs e)
        {
            int SelectIndex = this.TalentListView.SelectedIndex;
            if (SelectIndex == -1)
                return;
            this.TalentJArray.RemoveAt(SelectIndex);
            this.TalentListView.Items.RemoveAt(SelectIndex);
            //this.npc.NeigongList=this.NeigongJArray;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            this.npc.NeigongList = this.NeigongJArray;
            this.npc.TalentList = this.TalentJArray;
            this.Close();
        }

        private void AddNeiGongButton_Click(object sender, RoutedEventArgs e)
        {
            if (NeigongJArray.Count >= 6)
            {
                MessageBox.Show("内功已满", "提示");
                return;
            }
            AddSomeThing addTeammate = new AddSomeThing(2);
            addTeammate.Top = this.Top + this.Height / 3;
            addTeammate.Left = this.Left + this.Width / 3;
            addTeammate.ShowDialog();
            if (String.IsNullOrEmpty(addTeammate.ResultStr))
                return;
            String NeiGongID = SaveFix.ConverID(addTeammate.ResultStr, 2);
            foreach(JToken neigong in NeigongJArray)
            {
                if(neigong["iSkillID"].ToString()==NeiGongID)
                {
                    MessageBox.Show("已存在该内功!","错误");
                    return;
                }
            }
            JToken newNeigong = new JObject();
            newNeigong["bUse"] = "false";
            newNeigong["m_iAccumulationExp"]="0";
            newNeigong["iSkillID"]=NeiGongID;
            newNeigong["iLevel"]="1";
            NeigongJArray.Add(newNeigong);
            this.Initial();
        }

        private void AddTalentButton_Click(object sender, RoutedEventArgs e)
        {
            if (TalentJArray.Count >= 4)
            {
                MessageBox.Show("天赋已满", "提示");
                return;
            }
            AddSomeThing addTeammate = new AddSomeThing(3);
            addTeammate.Top = this.Top + this.Height / 3;
            addTeammate.Left = this.Left + this.Width / 3;
            addTeammate.ShowDialog();
            if (String.IsNullOrEmpty(addTeammate.ResultStr))
                return;
            String TalentID = SaveFix.ConverID(addTeammate.ResultStr, 3);
            TalentJArray.Add(TalentID);
            this.Initial();
        }
    }
}
