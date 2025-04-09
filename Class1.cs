using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

class CameraCapture
{
    private FilterInfoCollection videoDevices;
    private VideoCaptureDevice videoSource;
    private PictureBox pictureBox;

    public CameraCapture(PictureBox pictureBox)
    {
        this.pictureBox = pictureBox;
        // 枚举所有可用的视频输入设备
        videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        if (videoDevices.Count == 0)
        {
            throw new Exception("未找到可用的摄像头设备。");
        }
        // 使用第一个摄像头设备
        videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
        videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
    }

    public void StartCamera()
    {
        try
        {
            videoSource.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"启动摄像头时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void StopCamera()
    {
        if (videoSource.IsRunning)
        {
            videoSource.SignalToStop();
            videoSource.WaitForStop();
        }
    }

    public void CaptureAndSaveImage(string savePath)
    {
        try
        {
            // 确保保存路径的文件夹存在
            string directory = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 从最后一帧图像中保存到指定路径
            if (lastFrame != null)
            {
                lastFrame.Save(savePath);
                MessageBox.Show($"图像已保存到: {savePath}", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("未能捕获到图像。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (lastFrame != null)
            {
                lastFrame.Dispose();
            }
        }
    }

    private Bitmap lastFrame;
    private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
    {
        try
        {
            // 获取最新的帧图像
            lastFrame = (Bitmap)eventArgs.Frame.Clone();
            if (lastFrame != null)
            {
                if (pictureBox.InvokeRequired)
                {
                    pictureBox.Invoke(new Action(() =>
                    {
                        if (lastFrame != null)
                        {
                            pictureBox.Image = lastFrame;
                        }
                    }));
                }
                else
                {
                    if (lastFrame != null)
                    {
                        pictureBox.Image = lastFrame;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"更新图像时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

class Program : Form
{
    private CameraCapture cameraCapture;
    private PictureBox pictureBox;
    private Button captureButton;

    public Program()
    {
        this.Text = "摄像头捕获";
        this.Size = new Size(256, 256);

        pictureBox = new PictureBox();
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        this.Controls.Add(pictureBox);

        captureButton = new Button();
        captureButton.Text = "捕获";
        captureButton.Dock = DockStyle.Bottom;
        captureButton.Click += CaptureButton_Click;
        this.Controls.Add(captureButton);

        try
        {
            cameraCapture = new CameraCapture(pictureBox);
            cameraCapture.StartCamera();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"初始化摄像头时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void CaptureButton_Click(object sender, EventArgs e)
    {
        // 指定保存图像的路径
        string savePath = @"C:\CapturedImages\captured_image.jpg";
        cameraCapture.CaptureAndSaveImage(savePath);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (cameraCapture != null)
        {
            cameraCapture.StopCamera();
        }
        base.OnFormClosing(e);
    }

    //[STAThread]
    //static void Main()
    //{
    //    Application.EnableVisualStyles();
    //    Application.Run(new Program());
    //}
}

