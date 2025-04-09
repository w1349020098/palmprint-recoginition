using Sunny.UI;
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
    public partial class Adduser: UIForm
    {
        public UIForm manager = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 用于各种数据操作 自定义类
        /// </summary>
        sqlcontrol msop = null;
        public Adduser(UIForm fm)
        {
            manager = fm;
            InitializeComponent();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            int count = 0;

            var query = $"SELECT * FROM `user` WHERE name='{uiTextBox1.Text}';";//查重语句

            var countstr = "SELECT * FROM user;";//统计语句

            msop = new sqlcontrol(connectStr);
            msop.userselect(query);
            if (msop.userInfos.Count == 0 && uiTextBox1.Text != "")
            {
                msop.userselect(countstr);
                count = msop.userInfos.Count + 1;
                var addstr = $"INSERT INTO `palrc`.`user` (`id`,`name`,`password`) VALUES ('{count}','{uiTextBox1.Text}','123');";//添加新值
                msop.OpAddDeleUpdateMySql(addstr);
                UIMessageBox.Show("添加成功,初始密码123");

            }
            else
            {
                UIMessageBox.ShowError("已存在");
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            manager.Show();
            this.Close();
        }
    }
}
