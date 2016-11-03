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
    /// ChangeLevel.xaml 的交互逻辑
    /// </summary>
    public partial class ChangeLevel : Window
    {
        JToken NeiGong;
        public ChangeLevel(JToken NeiGong)
        {
            this.NeiGong = NeiGong;
            InitializeComponent();
            this.initial();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int NeiGLevelInt = 0;
            try
            {
                NeiGLevelInt = Convert.ToInt32(this.NeiLV.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show(this,"填入等级有误!");
                return;
            }
            if (NeiGLevelInt>10||NeiGLevelInt<1)
            {
                MessageBox.Show(this, "填入等级有误!");
                return;
            }
            this.NeiGong["iLevel"] = NeiGLevelInt;
            int expLevel = 0;
            if (NeiGLevelInt < 5)
                expLevel = 2000;
            else expLevel = 4000;
            this.NeiGong["m_iAccumulationExp"] = (NeiGLevelInt - 1) *expLevel;
            this.Close();
        }
        private void initial()
        {
            this.NeiGongName.Content = SaveFix.ConverID(this.NeiGong["iSkillID"].ToString(),2)+":    Lv";
            this.NeiLV.Text = this.NeiGong["iLevel"].ToString();
        }
    }
}
