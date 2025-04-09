using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalRecSystem
{
    public partial class Addmanager: UIForm
    {

        public UIForm managerst = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 注册码
        /// </summary>
        public string mestr = "3568";
        /// <summary>
        /// 用于各种数据操作 自定义类
        /// </summary>
        sqlcontrol msop = null;

        public Addmanager(UIForm fm)
        {
            managerst = fm;
            InitializeComponent();
        }



        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            int count = 0;
            
            var query = $"SELECT * FROM `manager` WHERE name='{uiTextBox1.Text}';";//查重语句

            var countstr = "SELECT * FROM manager;";//统计语句

            if(uiTextBox3.Text== mestr)
            {

                msop = new sqlcontrol(connectStr);
                msop.managerSelect(query);
                if (msop.managerInfos.Count == 0 && uiTextBox1.Text != "")
                {
                    msop.managerSelect(countstr);
                    count = msop.managerInfos.Count+1;
                    var addstr = $"INSERT INTO `palrc`.`manager` (`id`,`name`,`password`) VALUES ('{count}','{uiTextBox1.Text}','{uiTextBox2.Text}');";//添加新值
                    msop.OpAddDeleUpdateMySql(addstr);
                    UIMessageBox.Show("注册成功");

                }
                else
                {
                    UIMessageBox.ShowError("已存在");
                }

            }
            else
            {
                UIMessageBox.ShowError("注册码不正确");
            }



        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            managerst.Show();
            this.Close();
        }

        private void Addmanager_Load(object sender, EventArgs e)
        {

        }
    }
}
