using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImgEditLiteWPF
{
    //调色抽象类
    public abstract class Toning
    {
        //应用调色
        public abstract Bitmap DoToning(Bitmap img, int n);
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

    //亮度
    public class LDToning : Toning
    {
        public override Bitmap DoToning(Bitmap img, int n)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            double ld = 1 + Convert.ToDouble(n) / 10;
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

    //对比度
    public class DBDToning : Toning
    {
        public override Bitmap DoToning(Bitmap img, int n)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            double dbd = 1 + Convert.ToDouble(n) / 10;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(((pixel.R / 255.0 - 0.5) * dbd + 0.5) * 255);
                    green = (int)(((pixel.G / 255.0 - 0.5) * dbd + 0.5) * 255);
                    blue = (int)(((pixel.B / 255.0 - 0.5) * dbd + 0.5) * 255);
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return img;
        }
    }

    //饱和度
    public class BHDToning : Toning
    {
        public override Bitmap DoToning(Bitmap img, int n)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            double srz = Convert.ToDouble(n);
            double bhd = srz / 20;
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    for (int y = 0; y < img.Height; y++)
                    {
                        pixel = img.GetPixel(x, y);
                        red = pixel.R;
                        green = pixel.G;
                        blue = pixel.B;
                        double max = Math.Max(red, Math.Max(green, blue));
                        double min = Math.Min(red, Math.Min(green, blue));
                        double delta, value;
                        delta = (max - min) / 255;
                        value = (max + min) / 255;
                        double new_r, new_g, new_b;
                        if (delta == 0)      // 差为 0 不做操作，保存原像素点
                        {
                            continue;
                        }

                        double light, sat, alpha;
                        light = value / 2;

                        if (light < 0.5)
                            sat = delta / value;
                        else
                            sat = delta / (2 - value);

                        if (bhd >= 0)
                        {
                            if ((bhd + sat) >= 1)
                                alpha = sat;
                            else
                            {
                                alpha = 1 - bhd;
                            }
                            alpha = 1 / alpha - 1;
                            new_r = red + (red - light * 255) * alpha;
                            new_g = green + (green - light * 255) * alpha;
                            new_b = blue + (blue - light * 255) * alpha;
                        }
                        else
                        {
                            alpha = bhd;
                            new_r = light * 255 + (red - light * 255) * (1 + alpha);
                            new_g = light * 255 + (green - light * 255) * (1 + alpha);
                            new_b = light * 255 + (blue - light * 255) * (1 + alpha);
                        }
                        red = Convert.ToInt32(new_b);
                        green = Convert.ToInt32(new_g);
                        blue = Convert.ToInt32(new_r);
                        red = Judge(red);
                        green = Judge(green);
                        blue = Judge(blue);
                        img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
            }
            return img;
        }
    }

    //色温
    public class SWToning : Toning
    {
        public override Bitmap DoToning(Bitmap img, int n)
        {
            img = new Bitmap(img);
            Color pixel;
            int red, green, blue;
            double srz = Convert.ToDouble(n);
            double swr, swg, swb;
            if (srz >= 0)
            {
                swr = 1 + srz / 30;
                swg = 1 + srz / 60;
                swb = 1 + srz / 60;
            }
            else
            {
                swr = 1 - srz / 150;
                swg = 1 - srz / 100;
                swb = 1 - srz / 20;
            }
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = img.GetPixel(x, y);
                    red = (int)(pixel.R * swr);
                    green = (int)(pixel.G * swg);
                    blue = (int)(pixel.B * swb);
                    red = Judge(red);
                    green = Judge(green);
                    blue = Judge(blue);
                    img.SetPixel(x, y, Color.FromArgb(red, green, blue));
                }
            }
            return img;
        }
    }

    //调色工厂
    public class ToningFactory
    {
        public static Toning getToning(string str)
        {
            if (str == "亮度")
            {
                return new LDToning();  //亮度
            }
            else if (str == "对比度")
            {
                return new DBDToning();  //对比度
            }
            else if (str == "饱和度")
            {
                return new BHDToning();  //饱和度
            }
            else if (str == "色温")
            {
                return new SWToning();  //色温
            }
            else
            {
                return null;
            }
        }
    }
}
