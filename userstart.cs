using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalRecSystem
{
    public partial class userstart: UIForm
    {
        public static string name;
        public UIForm fm1 = null;
        public userstart(start start)
        {
            fm1 = start;
            InitializeComponent();
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 数据库user表查询语句，用户登录
        /// </summary>
        private string operatorStr = "select name,password from user;";

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
            else
            {
                name = uiTextBox1.Text;
                this.Hide();
                new user(name, fm1).Show();
            }


        }
        private void uiButton1_Click(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            LoginSystem();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            fm1.Show();
            Close();
            
        }
    }
}
