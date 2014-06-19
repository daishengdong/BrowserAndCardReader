using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace BrowserAndCardReader
{
    public partial class MainFrm : Form
    {
        string frmTextOpen = "身份证信息读取及自动填写系统 v2.1 --设备已打开";
        string frmTextClose = "身份证信息读取及自动填写系统 v2.1 --设备未打开";
        bool opened = false;
        Card2.PERSONINFOW person;
        string birthday = "";
        string validDate = "";
        Image image = null;
        const int maxErrorTextLen = 32;

        bool consecutiveAdd = true;

        List<WebBrowser> webNew = new List<WebBrowser>();
        private String ur1;

        public MainFrm()
        {
            InitializeComponent();

            person = new Card2.PERSONINFOW();
            this.homePage();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = frmTextClose;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // 打开设备
            if (opened)
            {
                return;
            }
            Int32 result;
            StringBuilder errorText = new StringBuilder(maxErrorTextLen);
            /*参数1为端口号。1表示串口1，2表示串口2，依次类推。1001表示USB。0表示自动选择。
              参数2为标志位。0x02表示启用重复读卡。0x04表示读卡后接着读取新地址。
              各个数值可以用“按位或”运算符组合起来。
              参数3为波特率。使用串口阅读器的程序应正确设置此参数。出厂机器的波特率一般为115200。
            */
            result = Card2.OpenCardReader(1001, 2, 115200);
            if (result == 0)
            {
                // textResult.Text = Convert.ToString(result);
                Card2.GetErrorTextW(errorText, maxErrorTextLen);
                opened = true;
                this.Text = frmTextOpen;
                // textDescription.Text = errorText.ToString();
            }
            else
            {
                MessageBox.Show("设备连接失败！\n请检查并确认设备已正确连接", "设备连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // 关闭设备
            if (!opened)
            {
                return;
            }
            Int32 result;
            StringBuilder errorText = new StringBuilder(maxErrorTextLen);
            //读完卡后，必须关闭设备。
            result = Card2.CloseCardReader();
            // textResult.Text = Convert.ToString(result);
            Card2.GetErrorTextW(errorText, maxErrorTextLen);
            opened = false;
            this.Text = frmTextClose;
            // textDescription.Text = errorText.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // 读卡并填写
            if (!opened)
            {
                MessageBox.Show("请先打开设备！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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
            // textResult.Text = Convert.ToString(result);
            Card2.GetErrorTextW(errorText, maxErrorTextLen);
            // textDescription.Text = errorText.ToString();

            if (0 == result)
            {
                birthday = ConvertDate(person.birthday, 1);
                validDate = ConvertDate(person.validStart, 2) + "-";
                validDate += ConvertDate(person.validEnd, 2);
                image = new Bitmap(imagePath);
            }
            else
            {
                // birthday = "";
                // validDate = "";
                MessageBox.Show("请将身份证正确放置在读卡器上！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                webNew[tabControl1.SelectedIndex].Document.GetElementById("zjhm").SetAttribute("value", person.cardId);
                webNew[tabControl1.SelectedIndex].Document.GetElementById("xm").SetAttribute("value", person.name);
                webNew[tabControl1.SelectedIndex].Document.GetElementById("csrq").SetAttribute("value", birthday);

                string address = person.address;
                string province = "四川省";
                string modifiedAddr = address;

                if (address.Contains(province))
                {
                    modifiedAddr = string.Empty;
                    int index = address.IndexOf(province);
                    modifiedAddr = address.Insert(index + province.Length, "达州市");
                }

                webNew[tabControl1.SelectedIndex].Document.GetElementById("djzsxx").SetAttribute("value", modifiedAddr);
                webNew[tabControl1.SelectedIndex].Document.GetElementById("lxzsxx").SetAttribute("value", modifiedAddr);

                webNew[tabControl1.SelectedIndex].Document.GetElementById("yzbm").SetAttribute("value", "635200");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请打开至正确网页！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                    return string.Format("{0}-{1}-{2}", year, month, day);
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
                        return string.Format("{0}-{1}-{2}", year, month, day);
                    }
                }
            }
            return "";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // 打开新页
            if (webNew[tabControl1.SelectedIndex].Url.ToString().Equals("http://www.scjj.gov.cn:8635/index.aspx"))
            {
                this.tabControl1.TabPages.Add("              ");
                this.webNew.Add(new WebBrowser());
                this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
                this.tabControl1.TabPages[this.tabControl1.TabPages.Count - 1].Controls.Add(webNew[tabControl1.SelectedIndex]);
                webNew[tabControl1.SelectedIndex].Dock = DockStyle.Fill;
                webNew[tabControl1.SelectedIndex].Navigate("http://www.scjj.gov.cn:8635/lr_add.aspx");
                this.toolStripComboBox1.Text = "http://www.scjj.gov.cn:8635/lr_add.aspx";
            }
            else
            {
                MessageBox.Show("请先登录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void homePage()
        {
            try
            {
                KeyEventArgs ev = new KeyEventArgs(Keys.Enter);
                object sender = new object();
                toolStripComboBox1_KeyDown(sender, ev);
                webNew[tabControl1.SelectedIndex].Navigate("http://www.scjj.gov.cn:8635/login.aspx");
                this.toolStripComboBox1.Text = "http://www.scjj.gov.cn:8635/login.aspx";
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            // 后退
            try
            {
                this.webNew[tabControl1.SelectedIndex].GoBack();
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // 前进
            try
            {
                this.webNew[tabControl1.SelectedIndex].GoForward();
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.ur1 = this.toolStripComboBox1.Text.Trim();
                    if (ur1.StartsWith("www.") || ur1.StartsWith("WWW."))
                        ur1 = "http://" + ur1 + @"/";
                    if (ur1.StartsWith("http://") || ur1.StartsWith("ftp://"))
                        this.toolStripComboBox1.Text = ur1;


                    if (this.tabControl1.TabPages.Count == 0)
                    {
                        this.webNew.Add(new WebBrowser());
                        this.tabControl1.TabPages.Add("", "正在载入……", 0);
                        this.tabControl1.TabPages[0].Controls.Add(webNew[tabControl1.SelectedIndex]);
                        this.webNew[tabControl1.SelectedIndex].NewWindow += new System.ComponentModel.CancelEventHandler(this.webNew_NewWindow);
                        this.webNew[tabControl1.SelectedIndex].DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webNew_DocumentCompleted);
                    }
                    this.tabControl1.TabPages[tabControl1.SelectedIndex].Text = "正在载入……";
                    webNew[tabControl1.SelectedIndex].Dock = DockStyle.Fill;
                    this.webNew[tabControl1.SelectedIndex].Navigate(ur1);
                    this.toolStripButton11.Enabled = true;
                    this.toolStripButton10.Enabled = true;

                    bool IsAdd = false;
                    for (int i = 0; i < toolStripComboBox1.Items.Count; i++)
                    {
                        if (this.ur1.Equals((String)toolStripComboBox1.Items[i]))
                        { IsAdd = true; break; }
                    }
                    if (!IsAdd)
                    {
                        toolStripComboBox1.Items.Add(this.ur1);
                    }
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void webNew_NewWindow(object sender, CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
                this.tabControl1.TabPages.Add("", "正在载入……", 0);
                this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
                this.webNew.Add(new WebBrowser());
                this.tabControl1.TabPages[this.tabControl1.TabPages.Count - 1].Controls.Add(webNew[tabControl1.SelectedIndex]);
                webNew[tabControl1.SelectedIndex].Dock = DockStyle.Fill;
                WebBrowser srcBrowser = (WebBrowser)sender;
                string newUrl = srcBrowser.StatusText;
                this.toolStripComboBox1.Text = newUrl;
                webNew[tabControl1.SelectedIndex].Navigate(newUrl);
                // this.toolStripButton10.Enabled = true;
                // this.toolStripButton8.Enabled = true;
                this.webNew[tabControl1.SelectedIndex].NewWindow += new System.ComponentModel.CancelEventHandler(this.webNew_NewWindow);
                this.webNew[tabControl1.SelectedIndex].DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webNew_DocumentCompleted);

            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void webNew_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string a = this.webNew[tabControl1.SelectedIndex].DocumentTitle + ("……………………");
            this.tabControl1.TabPages[tabControl1.SelectedIndex].Text = a.Substring(0, 6);
            this.toolStripComboBox1.Text = this.webNew[tabControl1.SelectedIndex].Url.ToString();
            PanDuan();
            //   this.tabControl1.Size = new Size();
        }

        private void PanDuan()
        {
            if (this.webNew[tabControl1.SelectedIndex].CanGoBack)
            {
                this.toolStripButton5.Enabled = true;
            }
            else
            {
                this.toolStripButton5.Enabled = false;
            }
            if (this.webNew[tabControl1.SelectedIndex].CanGoForward)
            {
                this.toolStripButton6.Enabled = true;
            }
            else
            {
                this.toolStripButton6.Enabled = false;
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            KeyEventArgs ev = new KeyEventArgs(Keys.Enter);
            toolStripComboBox1_KeyDown(sender, ev);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // 停止
            try
            {
                webNew[tabControl1.SelectedIndex].Stop();//停止
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            // 刷新
            try
            {
                webNew[tabControl1.SelectedIndex].Refresh();
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            // 主页
            this.homePage();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            // 关闭全部
            try
            {
                if (this.tabControl1.TabCount > 0)
                {
                    webNew.Clear();
                    tabControl1.TabPages.Clear();
                    this.toolStripComboBox1.Text = "";
                    this.toolStripButton5.Enabled = false;
                    this.toolStripButton6.Enabled = false;
                    this.toolStripButton11.Enabled = false;
                    this.toolStripButton10.Enabled = false;
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            // 关闭当前
            try
            {
                this.webNew[tabControl1.SelectedIndex].Dispose();
                this.webNew.Remove(this.webNew[tabControl1.SelectedIndex]);
                tabControl1.TabPages[tabControl1.SelectedIndex].Dispose();
                if (tabControl1.SelectedIndex >= 0)
                    PanDuan();
                else
                {
                    this.toolStripComboBox1.Text = "";
                    this.toolStripButton5.Enabled = false;
                    this.toolStripButton6.Enabled = false;
                    this.toolStripButton11.Enabled = false;
                    this.toolStripButton10.Enabled = false;
                }
                if (consecutiveAdd)
                {
                    this.tabControl1.TabPages.Add("              ");
                    this.webNew.Add(new WebBrowser());
                    this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
                    this.tabControl1.TabPages[this.tabControl1.TabPages.Count - 1].Controls.Add(webNew[tabControl1.SelectedIndex]);
                    webNew[tabControl1.SelectedIndex].Dock = DockStyle.Fill;
                    webNew[tabControl1.SelectedIndex].Navigate("http://www.scjj.gov.cn:8635/lr_add.aspx");
                    this.toolStripComboBox1.Text = "http://www.scjj.gov.cn:8635/lr_add.aspx";
                }
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }

        private void MainFrm_Resize(object sender, EventArgs e)
        {
            this.toolStripComboBox1.Width = this.Width - 200;
        }

        private void 连续添加模式ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // 单个添加模式ToolStripMenuItem1
            if (!连续添加模式ToolStripMenuItem1.Checked)
            {
                连续添加模式ToolStripMenuItem1.Checked = true;
                return;
            }
            consecutiveAdd = true;
            单个添加模式ToolStripMenuItem1.Checked = false;
        }

        private void 单个添加模式ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!单个添加模式ToolStripMenuItem1.Checked)
            {
                单个添加模式ToolStripMenuItem1.Checked = true;
                return;
            }
            consecutiveAdd = false;
            连续添加模式ToolStripMenuItem1.Checked = false;
        }
    }
}
