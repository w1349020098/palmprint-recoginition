using OpenCvSharp;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace PalRecSystem
{
    class Rec_rel
    {
        // 预处理图像
        static Mat PreprocessImage(Mat image)
        {
            Mat gray = new Mat();
            // 灰度化
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            // 高斯模糊
            Cv2.GaussianBlur(gray, gray, new Size(5, 5), 0);
            // 直方图均衡化
            Cv2.EqualizeHist(gray, gray);
            return gray;
        }

        // 提取特征
        static Mat ExtractFeatures(Mat image)
        {
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            Mat absGradX = new Mat();
            Mat absGradY = new Mat();
            // 计算x方向梯度
            Cv2.Sobel(image, gradX, MatType.CV_16S, 1, 0, 3, 1, 0, BorderTypes.Default);
            Cv2.ConvertScaleAbs(gradX, absGradX);
            // 计算y方向梯度
            Cv2.Sobel(image, gradY, MatType.CV_16S, 0, 1, 3, 1, 0, BorderTypes.Default);
            Cv2.ConvertScaleAbs(gradY, absGradY);
            // 合并x和y方向梯度
            Mat gradient = new Mat();
            Cv2.AddWeighted(absGradX, 0.5, absGradY, 0.5, 0, gradient);

            // 形态学处理增强纹理信息
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
            Cv2.MorphologyEx(gradient, gradient, MorphTypes.Close, kernel);

            return gradient;
        }

        // 匹配特征
        static double MatchFeatures(Mat features1, Mat features2)
        {
            Mat result = new Mat();
            Cv2.MatchTemplate(features1, features2, result, TemplateMatchModes.CCoeffNormed);
            double minVal, maxVal;
            Point minLoc, maxLoc;
            Cv2.MinMaxLoc(result, out minVal, out maxVal, out minLoc, out maxLoc);
            return maxVal;
        }

        // 数据增强
        static List<Mat> DataAugmentation(Mat image)
        {
            List<Mat> augmentedImages = new List<Mat>();
            augmentedImages.Add(image.Clone());  // 原始图像

            Mat flippedHorizontally = new Mat();
            Cv2.Flip(image, flippedHorizontally, FlipMode.X);  // 水平翻转
            augmentedImages.Add(flippedHorizontally);

            Mat flippedVertically = new Mat();
            Cv2.Flip(image, flippedVertically, FlipMode.Y);  // 垂直翻转
            augmentedImages.Add(flippedVertically);

            return augmentedImages;
        }

        // 掌纹识别，计算相似度
        static double PalmprintRecognition(Mat inputImage, Mat templateImage)
        {
            // 预处理模板图像
            Mat templateProcessed = PreprocessImage(templateImage);
            Mat templateFeatures = ExtractFeatures(templateProcessed);

            // 对输入图像进行数据增强
            List<Mat> augmentedInputImages = DataAugmentation(inputImage);

            double maxSimilarity = 0.0;
            foreach (Mat augmentedImage in augmentedInputImages)
            {
                // 预处理增强后的输入图像
                Mat inputProcessed = PreprocessImage(augmentedImage);
                // 提取增强后的输入图像特征
                Mat inputFeatures = ExtractFeatures(inputProcessed);

                // 特征匹配
                double similarity = MatchFeatures(inputFeatures, templateFeatures);

                // 更新最大相似度
                if (similarity > maxSimilarity)
                {
                    maxSimilarity = similarity;
                }
            }

            return maxSimilarity;
        }

        // 计算一对图像的相似度
        static double CalculatePairSimilarity(string inputPath, string templatePath)
        {
            // 读取图像
            Mat inputImg = Cv2.ImRead(inputPath);
            Mat templateImg = Cv2.ImRead(templatePath);

            // 验证图像读取是否成功
            if (inputImg.Empty() || templateImg.Empty())
            {
                Console.WriteLine("无法读取图像，请检查文件路径：");
                Console.WriteLine("输入图像: " + inputPath);
                Console.WriteLine("模板图像: " + templatePath);
                return 0;
            }

            // 计算相似度，双向匹配取最大值
            double similarity1 = PalmprintRecognition(inputImg, templateImg);
            double similarity2 = PalmprintRecognition(templateImg, inputImg);
            double finalSimilarity = Math.Max(similarity1, similarity2);
            return finalSimilarity;
        }
        /// <summary>
        /// 最终相似度
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public double cal2(string s1, string s2)
        {
            string inputimagepath = s1;
            string templateimagepath = s2;

            double similarity = CalculatePairSimilarity(inputimagepath, templateimagepath);

            return similarity;
        }
    }
}