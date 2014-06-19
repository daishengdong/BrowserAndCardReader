using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Test2
{
    public partial class Form1 : Form
    {
        Card2.PERSONINFOW person;
        string birthday = "";
        string validDate = "";
        Image image = null;
        const int maxErrorTextLen = 32;

        public Form1()
        {
            InitializeComponent();
            person = new Card2.PERSONINFOW();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 result;
            StringBuilder errorText = new StringBuilder(maxErrorTextLen);
            /*参数1为端口号。1表示串口1，2表示串口2，依次类推。1001表示USB。0表示自动选择。
              参数2为标志位。0x02表示启用重复读卡。0x04表示读卡后接着读取新地址。
              各个数值可以用“按位或”运算符组合起来。
              参数3为波特率。使用串口阅读器的程序应正确设置此参数。出厂机器的波特率一般为115200。
            */
            result = Card2.OpenCardReader(0, 2, 115200);
            textResult.Text = Convert.ToString(result);
            Card2.GetErrorTextW(errorText, maxErrorTextLen);
            textDescription.Text = errorText.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Int32 result;
            String imagePath;
            StringBuilder errorText = new StringBuilder(maxErrorTextLen);

            if (image != null)
            {
                image.Dispose();
                image = null;
            }
            imagePath = Path.GetTempPath() + "image.bmp";
            //当程序打开设备后，可以多次调用读取信息函数。
            result = Card2.GetPersonMsgW(ref person, imagePath);
            textResult.Text = Convert.ToString(result);
            Card2.GetErrorTextW(errorText, maxErrorTextLen);
            textDescription.Text = errorText.ToString();

            if (0 == result)
            {
                birthday = ConvertDate(person.birthday, 1);
                validDate = ConvertDate(person.validStart, 2) + "-";
                validDate += ConvertDate(person.validEnd, 2);
                image = new Bitmap(imagePath);
            }
            else
            {
                birthday = "";
                validDate = "";
            }
            Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int32 result;
            StringBuilder errorText = new StringBuilder(maxErrorTextLen);
            //读完卡后，必须关闭设备。
            result = Card2.CloseCardReader();
            textResult.Text = Convert.ToString(result);
            Card2.GetErrorTextW(errorText, maxErrorTextLen);
            textDescription.Text = errorText.ToString();
        }

        void SetRect(ref RectangleF rect, float x, float y, float width, float height)
        {
            rect.X = x;
            rect.Y = y;
            rect.Width = width;
            rect.Height = height;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            Font font = new Font("宋体", 12, FontStyle.Regular);
            Brush brush = new SolidBrush(Color.FromArgb(192, 128, 128));
            RectangleF rect = new RectangleF();
            SetRect(ref rect, 10, 10, 90, 16);
            g.DrawString("姓名", font, brush, rect);
            SetRect(ref rect, 10, 30, 90, 16);
            g.DrawString("性别", font, brush, rect);
            SetRect(ref rect, 10, 50, 90, 16);
            g.DrawString("民族", font, brush, rect);
            SetRect(ref rect, 10, 70, 90, 16);
            g.DrawString("出生日期", font, brush, rect);
            SetRect(ref rect, 10, 90, 90, 16);
            g.DrawString("地址", font, brush, rect);
            SetRect(ref rect, 10, 130, 90, 16);
            g.DrawString("身份证号码", font, brush, rect);
            SetRect(ref rect, 10, 150, 90, 16);
            g.DrawString("签发机关", font, brush, rect);
            SetRect(ref rect, 10, 170, 90, 16);
            g.DrawString("有效期限", font, brush, rect);
            brush.Dispose();

            if (person.name != null)
            {
                brush = new SolidBrush(Color.Black);
                SetRect(ref rect, 100, 10, 300, 16);
                g.DrawString(person.name, font, brush, rect);
                SetRect(ref rect, 100, 30, 100, 16);
                g.DrawString(person.sex, font, brush, rect);
                SetRect(ref rect, 100, 50, 150, 16);
                g.DrawString(person.nation, font, brush, rect);
                SetRect(ref rect, 100, 70, 300, 16);
                g.DrawString(birthday, font, brush, rect);
                SetRect(ref rect, 100, 90, 300, 34);
                g.DrawString(person.address, font, brush, rect);
                SetRect(ref rect, 100, 130, 300, 16);
                g.DrawString(person.cardId, font, brush, rect);
                SetRect(ref rect, 100, 150, 300, 16);
                g.DrawString(person.police, font, brush, rect);
                SetRect(ref rect, 100, 170, 300, 16);
                g.DrawString(validDate, font, brush, rect);
                brush.Dispose();
            }
            font.Dispose();
            if (image != null)
                g.DrawImage(image, 400, 10);
            g.Dispose();
        }

        string ConvertDate(string str, int mode)
        {
            string year;
            string month;
            string day;
            if (1 == mode)
            {
                if (str.Length >= 8)
                {
                    year = str.Substring(0, 4);
                    month = str.Substring(4, 2);
                    day = str.Substring(6, 2);
                    return string.Format("{0}年{1}月{2}日", year, month, day);
                }
            }
            else if (2 == mode)
            {
                if (str.Equals("长期"))
                {
                    return "长期";
                }
                else
                {
                    if (str.Length >= 8)
                    {
                        year = str.Substring(0, 4);
                        month = str.Substring(4, 2);
                        day = str.Substring(6, 2);
                        return string.Format("{0}.{1}.{2}", year, month, day);
                    }
                }
            }
            return "";
        }
    }
}
