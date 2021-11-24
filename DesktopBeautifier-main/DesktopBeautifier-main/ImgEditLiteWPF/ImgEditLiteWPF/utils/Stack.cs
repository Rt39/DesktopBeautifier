using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ImgEditLiteWPF
{
    //用于撤销恢复的栈
    class Stack
    {
        private const int maxSize = 100;                    //栈的最大值

        private int top = -1;                               //栈底

        private StackImg[] stacks = new StackImg[maxSize];  //栈

        //单例对象
        private static Stack stack = new Stack();

        //获取单例对象
        public static Stack getStack()
        {
            return stack;
        }

        //构造函数
        private Stack() { }

        //入栈
        public bool push(StackImg s)
        {
            stacks[++top] = s;
            return true;
        }

        //出栈
        public StackImg pop()
        {
            if (isUndoable())
            {
                return stacks[--top];
            }
            return new StackImg();
        }

        //恢复
        public StackImg redo()
        {
            if (isRedoable())
            {
                return stacks[++top];
            }
            return new StackImg();
        }

        //是否可撤销
        public bool isUndoable()
        {
            if (top > 0)
            {
                return true;
            }
            return false;
        }

        //是否可恢复
        public bool isRedoable()
        {
            if (stacks[top + 1] != null)
            {
                return true;
            }
            return false;
        }
    }

    //用于保存撤销恢复的数据结构
    class StackImg
    {
        public int LD { get; set; }         //亮度
        public int DBD { get; set; }        //对比度
        public int BHD { get; set; }        //饱和度
        public int SW { get; set; }         //色温
        public Bitmap img { get; set; }     //图片

        //构造函数
        public StackImg()
        {

        }
        public StackImg(int LD, int DBD, int HD, int SD, Bitmap img)
        {
            this.LD = LD;
            this.DBD = DBD;
            this.BHD = HD;
            this.SW = SD;
            this.img = img;
        }
    }
}
