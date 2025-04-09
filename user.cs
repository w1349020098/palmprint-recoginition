using Mysqlx.Connection;
using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalRecSystem
{
    public partial class user: UIForm
    {
        public static string name = null;
        public UIForm fm1 = null;
        public user(string s , UIForm start)
        {
            fm1 = start;
            name = s;
            InitializeComponent();
        }

        public user()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";


        private Image CreateCustomImage()
        {
            // 创建一个指定大小的位图
            Bitmap bitmap = new Bitmap(200, 100);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 设置高质量的文本渲染
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                // 填充背景为白色
                g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

                // 设置字体和格式
                Font font = new Font("Arial", 20, FontStyle.Bold);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                // 在图片中心绘制文本
                g.DrawString("请录入", font, Brushes.Black, new RectangleF(0, 0, bitmap.Width, bitmap.Height), format);
            }
            return bitmap;
        }
        private void user_Load(object sender, EventArgs e)
        { 
            var operatorStr = $"SELECT *  FROM `user`  WHERE `user`.name = '{name}';";//数据库user表查询语句
            sqlcontrol msoph = new sqlcontrol(connectStr);
            msoph.userselect(operatorStr);
            uiTextBox1.Text = name;
            uiTextBox2.Text = msoph.userInfos[0].password;
            try
            {
                // 尝试获取图片路径
                string imagePath = msoph.userInfos[0].pal;

                // 检查文件是否存在
                if (File.Exists(imagePath))
                {
                    try
                    {
                        // 尝试加载图片
                        pictureBox1.Image = Image.FromFile(imagePath);
                    }
                    catch (Exception ex)
                    {
                        // 若加载失败，记录错误信息并显示自定义图片
                        Console.WriteLine($"加载图片时出错: {ex.Message}");
                        pictureBox1.Image = CreateCustomImage();
                    }
                }
                else
                {
                    // 文件不存在，显示自定义图片
                    pictureBox1.Image = CreateCustomImage();
                }
            }
            catch (Exception ex)
            {
                // 处理其他可能的异常，如 userInfos 为空等
                Console.WriteLine($"发生错误: {ex.Message}");
                pictureBox1.Image = CreateCustomImage();
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string nameValue = uiTextBox1.Text;
            string passwordValue = uiTextBox2.Text;
            var upstr = $"UPDATE `palrc`.`user` SET `name` = '{nameValue}', `password` = '{passwordValue}' WHERE `name` = '{name}'";
            if (uiTextBox1.Text!=""&& uiTextBox2.Text != "")
            {
                sqlcontrol msoph = new sqlcontrol(connectStr);
                msoph.OpAddDeleUpdateMySql(upstr);
                //MessageBox.Show(upstr);
                MessageBox.Show("修改成功");
            }
            else
            {
                MessageBox.Show("不能为空");
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fm1.Show();
            Close();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            capture cap = new capture(fm1,name);
            cap.Show();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void user_VisibleChanged(object sender, EventArgs e)
        {
            //InitializeComponent();
        }

    }
}
