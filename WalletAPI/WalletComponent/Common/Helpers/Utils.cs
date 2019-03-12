using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace WalletComponent.Common.Helpers
{
    public class Utils
    {
        public static byte[] StreamToBytes(Stream stream)

        {

            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);

            return bytes;

        }
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
        public static byte[] GenThumbnail(byte[] srcImage, int width, int height, out int realWidth, out int realHeight)
        {
            Image imageFrom = null;
            using (MemoryStream ms = new MemoryStream(srcImage))
            {
                ms.Seek(0, SeekOrigin.Begin);
                imageFrom = Image.FromStream(ms);
            }
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;

            if (imageFromWidth <= width && imageFromHeight <= height)
            {
                realHeight = imageFromHeight;
                realWidth = imageFromWidth;
                return srcImage;
            }
            // 生成的缩略图实际宽度及高度 
            int bitmapWidth = width;
            int bitmapHeight = height;
            // 生成的缩略图在上述"画布"上的位置 
            int X = 0;
            int Y = 0;
            // 根据源图及欲生成的缩略图尺寸,计算缩略图的实际尺寸及其在"画布"上的位置 
            if (bitmapHeight * imageFromWidth > bitmapWidth * imageFromHeight)
            {
                bitmapHeight = imageFromHeight * width / imageFromWidth;
                //Y = (height - bitmapHeight) / 2;
            }
            else
            {
                bitmapWidth = imageFromWidth * height / imageFromHeight;
                //X = (width - bitmapWidth) / 2;
            }

            realWidth = bitmapWidth;
            realHeight = bitmapHeight;
            // 创建画布
            Bitmap bmp = new Bitmap(bitmapWidth, bitmapHeight);
            Graphics g = Graphics.FromImage(bmp);
            // 用白色清空 
            g.Clear(Color.White);
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。 
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // 指定高质量、低速度呈现。 
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 在指定位置并且按指定大小绘制指定的 Image 的指定部分。 
            g.DrawImage(imageFrom, new Rectangle(X, Y, bitmapWidth, bitmapHeight), new Rectangle(0, 0, imageFromWidth, imageFromHeight), GraphicsUnit.Pixel);
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //经测试 .jpg 格式缩略图大小与质量等最优 
                    bmp.Save(ms, ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);

                    return ms.ToArray();
                }
            }
            finally
            {
                //显示释放资源 
                bmp.Dispose();
                g.Dispose();
            }
        }
    }
}
