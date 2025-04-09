using AForge.Video;
using AForge.Video.DirectShow;
using OpenCvSharp;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PalRecSystem
{
    public partial class Rec: UIForm
    {
        public FilterInfoCollection videoDevices;
        public VideoCaptureDevice videoSource;
        public UIForm fm1 = null;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string connectStr = "server=127.0.0.1;port=3306;user=root;password=123;database=palrc";
        /// <summary>
        /// 查询user表
        /// </summary>
        private string selStr = "SELECT * FROM `user`;";
        sqlcontrol msop = null;


        public Rec(UIForm uIForm)
        {
            fm1 = uIForm;
            InitializeComponent();
            this.FormClosing += capture_FormClosing;
        }

        private void capture_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止视频捕获
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // 获取新帧
            Bitmap videoFrame = (Bitmap)eventArgs.Frame.Clone();

            // 在PictureBox中显示新帧
            pictureBox1.Image = videoFrame;
        }

        public string Loop(string sel , string path)
        {
            double max = 0;
            double con = 0;
            string name = null;
            
            msop = new sqlcontrol(connectStr);
            msop.userselect(sel);
            Rec_rel rec_op = new Rec_rel();
            foreach (var i in msop.userInfos)
            {
                string path1 = Path.Combine("@", path);
                string path2 = Path.Combine("@", i.pal);
                //MessageBox.Show(path1, "山常志");
                //MessageBox.Show(path2, "关云长");

                con = rec_op.cal2(path, i.pal);
                if (max < con)
                {
                    max = rec_op.cal2(path, i.pal);
                    name = i.name;

                }
                //max = rec_op.cal2(path, i.pal);
                //name = i.name;
                uiLabel1.Text +="用户名："+ i.name+"相似度：" + con.ToString() + "\n";
            }
            return name;
        }

        private void Rec_Load(object sender, EventArgs e)
        {
            // 枚举所有视频输入设备
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count == 0)
            {
                MessageBox.Show("未找到视频输入设备！");
                return;
            }

            // 选择第一个视频输入设备
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            // 设置视频分辨率为256x256
            foreach (VideoCapabilities cap in videoSource.VideoCapabilities)
            {
                if (cap.FrameSize.Width == 512 && cap.FrameSize.Height == 512)
                {
                    videoSource.VideoResolution = cap;
                    break;
                }
            }
            // 设置新帧到达时的事件处理程序
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            // 开始捕获视频
            videoSource.Start();
        }
        //static Mat CropImage(Mat image)
        //{
        //    int width = image.Width;
        //    int height = image.Height;

        //    // 计算裁剪区域
        //    int startX = Math.Max(0, (width - 256) / 2);
        //    int startY = Math.Max(0, (height - 256) / 2);
        //    int endX = Math.Min(width, startX + 256);
        //    int endY = Math.Min(height, startY + 256);

        //    // 创建裁剪区域
        //    Rect roi = new Rect(startX, startY, endX - startX, endY - startY);

        //    // 裁剪图像
        //    Mat croppedImage = new Mat(image, roi);

        //    // 如果裁剪后的图像不是 256x256，则调整大小
        //    if (croppedImage.Width != 256 || croppedImage.Height != 256)
        //    {
        //        Mat resizedImage = new Mat();
        //        Cv2.Resize(croppedImage, resizedImage, new OpenCvSharp.Size(256, 256));
        //        croppedImage.Dispose();
        //        return resizedImage;
        //    }

        //    return croppedImage;
        //}
        static Mat CropImage(Mat image)
        {
            int width = image.Width;
            int height = image.Height;

            // 计算裁剪区域
            int startX = Math.Max(0, (width - 512) / 2);
            int startY = Math.Max(0, (height - 512) / 2);
            int endX = Math.Min(width, startX + 512);
            int endY = Math.Min(height, startY + 512);

            // 创建裁剪区域
            Rect roi = new Rect(startX, startY, endX - startX, endY - startY);

            // 裁剪图像
            Mat croppedImage = new Mat(image, roi);

            // 如果裁剪后的图像不是 512x512，则调整大小
            if (croppedImage.Width != 512 || croppedImage.Height != 512)
            {
                Mat resizedImage = new Mat();
                Cv2.Resize(croppedImage, resizedImage, new OpenCvSharp.Size(512, 512));
                croppedImage.Dispose();
                return resizedImage;
            }

            return croppedImage;
        }
        static void SaveImage(Mat image, string filePath)
        {
            try
            {
                Cv2.ImWrite(filePath, image);
                //MessageBox.Show($"图像已保存到: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存图像时出错: {ex.Message}");
            }
        }
        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // 生成保存文件的路径
                string subFolder = "RecImages";
                string startupPath = Application.StartupPath;
                string folderPath = Path.Combine(startupPath, subFolder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, $"capture_{DateTime.Now:yyyyMMddHHmmss}.jpg");

                // 保存当前画面到本地文件
                //pictureBox1.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                Mat matImage = OpenCvSharp.Extensions.BitmapConverter.ToMat((Bitmap)pictureBox1.Image);
                SaveImage(CropImage(matImage), filePath);

                string name= Loop(selStr, filePath);
                MessageBox.Show($"你好{name}", "识别成功");
            }


        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fm1.Show();
            this.Close();
        }
    }
}
