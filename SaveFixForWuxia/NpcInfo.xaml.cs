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
                talentItem.Height = 35;
                talentItem.HorizontalContentAlignment = HorizontalAlignment.Center;
                talentItem.Content=SaveFix.ConverID(talentID.ToString(), 3);
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
    }
}
