using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImgEditLiteWPF
{
    //滤镜抽象类
    public abstract class Filter
    {
        //应用滤镜
        public abstract Bitmap DoFilter(Bitmap img);
        //判断是否有越界的像素颜色值
        public int Judge(int i)
        {
            if (i >= 0 && i <= 255)
                return i;
            else if (i < 0)
                return 0;
            else
                return 255;
        }
    }

    //黑白

    public class HBFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Color pixel;
            int gray;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    gray = (int)(0.7 * pixel.R + 0.2 * pixel.G + 0.1 * pixel.B);
                    img.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            return img;
        }
    }

    //自然
    public class ZRFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(pixel.R * 1.2);
                    green = (int)(pixel.G * 1.2);
                    blue = (int)(pixel.B * 1.2);
                    if (red >= 225 && green >= 225 && blue >= 225)
                    {
                        red = (int)(red / 1.15);
                        green = (int)(green / 1.15);
                        blue = (int)(blue / 1.15);
                    }
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return img;
        }
    }

    //浮雕
    public class FDFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Color pixel, pixelnext;
            for (int x = 0; x < img.Width - 1; x++)
            {
                for (int y = 0; y < img.Height - 1; y++)
                {
                    int r, g, b;
                    pixel = img.GetPixel(x, y);
                    pixelnext = img.GetPixel(x + 1, y + 1);
                    r = pixel.R - pixelnext.R + 128;
                    b = pixel.B - pixelnext.B + 128;
                    g = pixel.G - pixelnext.G + 128;
                    r = Judge(r);
                    g = Judge(g);
                    b = Judge(b);
                    img.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return img;
        }
    }

    //青春
    public class QCFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(pixel.R * 1.1);
                    green = (int)(pixel.G * 1.03);
                    blue = (int)(pixel.B * 1);
                    if (red >= 225 && green >= 225 && blue >= 225)
                    {
                        red = (int)(red / 1.05);
                        green = (int)(green / 1.05);
                    }
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            float ld = 1 + 3 / 10;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(pixel.R * ld);
                    green = (int)(pixel.G * ld);
                    blue = (int)(pixel.B * ld);
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return img;
        }
    }

    //模糊
    public class MHFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Mat inmat = BitmapConverter.ToMat(img);
            Mat outmat = new Mat();
            Cv2.GaussianBlur(inmat, outmat, new OpenCvSharp.Size(11, 11), 11, 11);
            img = BitmapConverter.ToBitmap(outmat);
            return img;
        }
    }

    //静谧
    public class JMFilter : Filter
    {
        public override Bitmap DoFilter(Bitmap img)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(pixel.R * 1.1);
                    green = (int)(pixel.G * 1.1);
                    blue = (int)(pixel.B * 1);
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return img;
        }
    }

    //滤镜工厂
    public class FilterFactory
    {
        public static Filter getFilter(string str)
        {
            if (str == "黑白")
            {
                return new HBFilter();  //黑白
            }
            else if (str == "自然")
            {
                return new ZRFilter();  //自然
            }
            else if (str == "浮雕")
            {
                return new FDFilter();  //浮雕
            }
            else if (str == "青春")
            {
                return new QCFilter();  //青春
            }
            else if (str == "静谧")
            {
                return new JMFilter();  //静谧
            }
            else if (str == "模糊")
            {
                return new MHFilter();  //模糊
            }
            else
            {
                return null;
            }
        }
    }
}
