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
    public partial class manager: UIForm
    {
        public UIForm fm1 = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 数据库student表查询语句
        /// </summary>
        private string selStr = "SELECT * FROM `user`;";
        /// <summary>
        /// 用于各种数据操作 自定义类
        /// </summary>
        sqlcontrol msop = null;

        public manager(UIForm fm)
        {
            fm1 = fm;
            InitializeComponent();
        }
        /// <summary>
        /// 显示Data数据
        /// </summary>
        private void ShowInfo(string str)
        {
            msop = new sqlcontrol(connectStr);
            msop.userselect(str);
            // 数据源绑定
            uiDataGridView1.DataSource = msop.userInfos;

        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            new Adduser(this).Show();
            this.Hide();
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            fm1.Show();
            Close();
        }

        private void manager_Load(object sender, EventArgs e)
        {
            ShowInfo(selStr);
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            userInfo currentsel = msop.userInfos[uiDataGridView1.CurrentRow.Index];
            var delstr = $"DELETE FROM `palrc`.`user` WHERE `id` = '{currentsel.id}';";
            msop = new sqlcontrol(connectStr);
            msop.OpAddDeleUpdateMySql(delstr);
            UIMessageBox.ShowSuccess("删除成功");
            ShowInfo(selStr);
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            ShowInfo(selStr);
        }
    }
}
