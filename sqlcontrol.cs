using MySql.Data.MySqlClient;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalRecSystem
{
    class sqlcontrol
    {
        private string conStr = null;
        private MySqlConnection msc = null;
        private MySqlCommand msco = null;
        private MySqlDataReader msdr = null;
        // 用于系统登录 存储用户名和密码
        public Dictionary<string, string> dics = null;
        public List<userInfo> userInfos = null;
        public List<managerInfo> managerInfos = null;


        public sqlcontrol(string conStr)
        {
            this.conStr = conStr ?? throw new ArgumentNullException(nameof(conStr));
        }
        /// <summary>
        /// 系统 登录
        /// </summary>
        /// <param name="opStr"></param>
        public void OpLoginMySql(string queryStr)
        {
            try
            {
                msc = new MySqlConnection(conStr);
                msc.Open();
                msco = new MySqlCommand(queryStr, this.msc);
                msdr = msco.ExecuteReader();
                dics = new Dictionary<string, string>();
                while (msdr.Read())
                {
                    dics.Add(msdr[0].ToString(), msdr[1].ToString());
                }
            }
            catch
            {
                UIMessageBox.Show("登录失败！");
            }
            finally
            {
                msco.Dispose();
                msc.Close();
            }
        }
        /// <summary>
        /// 查询用户表
        /// </summary>
        /// <param name="queryStr"></param>
        public void userselect(string queryStr)
        {
            try
            {
                msc = new MySqlConnection(conStr);
                msco = new MySqlCommand(queryStr, this.msc);
                msc.Open();
                msdr = msco.ExecuteReader();
                userInfos = new List<userInfo>();
                while (msdr.Read())
                {
                    userInfos.Add(new userInfo(int.Parse(msdr[0].ToString()),
                        msdr[1].ToString(), msdr[2].ToString(), msdr[3].ToString()
                        ));
                }
                //foreach (var i in userInfos)
                //{
                //    MessageBox.Show(i.ToString());
                //}
            }
            catch
            {
                MessageBox.Show("用户查询失败！", "数据库查询", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msco.Dispose();
                msc.Close();
            }
        }
        /// <summary>
        /// 查询管理员表
        /// </summary>
        /// <param name="queryStr"></param>
        public void managerSelect(string queryStr)
        {
            try
            {
                msc = new MySqlConnection(conStr);
                msco = new MySqlCommand(queryStr, this.msc);
                msc.Open();
                msdr = msco.ExecuteReader();
                managerInfos = new List<managerInfo>();
                while (msdr.Read())
                {
                    managerInfos.Add(new managerInfo(int.Parse(msdr[0].ToString()),
                        msdr[1].ToString(), msdr[2].ToString()
                        ));
                }
                //foreach(var i in stus) {
                //    MessageBox.Show(i.ToString());
                //}
            }
            catch
            {
                
                MessageBox.Show("查询失败！", "数据库查询", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                msco.Dispose();
                msc.Close();
            }
        }
        /// <summary>
        /// 数据库操作 增删改
        /// </summary>
        /// <param name="opStr"></param>
        public void OpAddDeleUpdateMySql(string opStr)
        {

            msc = new MySqlConnection(conStr);
            msco = new MySqlCommand(opStr, this.msc);
            msc.Open();
            msco.ExecuteNonQuery();
            msco.Dispose();
            msc.Close();

        }
    }
}
