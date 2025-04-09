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
using System.Xml.Linq;

namespace PalRecSystem
{
    public partial class managerstart: UIForm
    {
        public UIForm fm1 = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 数据库manager表查询语句，管理员登录
        /// </summary>
        private string operatorStr = "select name,password from manager;";

        public managerstart(UIForm start)
        {
            fm1 = start;
            InitializeComponent();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        private void LoginSystem()
        {
            sqlcontrol msoph = new sqlcontrol(connectStr);
            msoph.OpLoginMySql(operatorStr);
            bool flag = false;

            foreach (var i in msoph.dics)
            {
                if (uiTextBox1.Text == i.Key && uiTextBox2.Text == i.Value)
                //if (true)
                {
                    flag = true;
                }
            }

            if (flag == false)
            {
                UIMessageBox.Show("认证失败！");
            }
            this.Hide();
            new manager(fm1).Show();

        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            LoginSystem();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            new Addmanager(this).Show();
            this.Hide();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            fm1.Show();
            Close();
        }
    }
}
