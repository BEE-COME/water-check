using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.IO.Ports;
using System.IO;
using System.Runtime.InteropServices;//引用命名空间
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Linq.Expressions;

namespace water
{
    public partial class FPage1 : UIPage
    {
        public static class IniFunc
        {
            /// <summary>
            /// 获取值
            /// </summary>
            /// <param name="section">段落名</param>
            /// <param name="key">键名</param>
            /// <param name="defval">读取异常是的缺省值</param>
            /// <param name="retval">键名所对应的的值，没有找到返回空值</param>
            /// <param name="size">返回值允许的大小</param>
            /// <param name="filepath">ini文件的完整路径</param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            private static extern int GetPrivateProfileString(
                string section,
                string key,
                string defval,
                StringBuilder retval,
                int size,
                string filepath);

            /// <summary>
            /// 写入
            /// </summary>
            /// <param name="section">需要写入的段落名</param>
            /// <param name="key">需要写入的键名</param>
            /// <param name="val">写入值</param>
            /// <param name="filepath">ini文件的完整路径</param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            private static extern int WritePrivateProfileString(
                string section,
                string key,
                string val,
                string filepath);


            /// <summary>
            /// 获取数据
            /// </summary>
            /// <param name="section">段落名</param>
            /// <param name="key">键名</param>
            /// <param name="def">没有找到时返回的默认值</param>
            /// <param name="filename">ini文件完整路径</param>
            /// <returns></returns>
            public static string getString(string section, string key, string def, string filename)
            {
                StringBuilder sb = new StringBuilder(1024);
                GetPrivateProfileString(section, key, def, sb, 1024, filename);
                return sb.ToString();
            }

            /// <summary>
            /// 写入数据
            /// </summary>
            /// <param name="section">段落名</param>
            /// <param name="key">键名</param>
            /// <param name="val">写入值</param>
            /// <param name="filename">ini文件完整路径</param>
            public static void writeString(string section, string key, string val, string filename)
            {
                WritePrivateProfileString(section, key, val, filename);
            }
        }

        public FPage1()
        {
            InitializeComponent();
        }
        int Row_count, time_count1, time_count2, main_state, recvice_flag, excel_count, flag_warn, time_count3,check_cnt;

        string recvice_data;

        string[] tmp = new string[10];

        int time_out = 30, jiexi_cnt = 0;


        private void uiSwitch1_ActiveChanged(object sender, EventArgs e)
        {
            //串口初始化            
            serialPort_init();

            //判断开启流程
            if (uiSwitch1.Active == Enabled)
            {

                //uiDataGridView1.ReadOnly = false;
                Main_proess();
            }
            else
            {
                timer1.Stop();
            }
        }

        private void uiSwitch1_ValueChanged(object sender, bool value)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)//1秒钟的定时器
        {

            if (recvice_flag == 1)
            {
                if (time_count2 > 0) time_count2--;

                if (time_count2 == 0)
                {
                    if (flag_warn == 1)
                    {
                        time_count3++;
                        if (time_count3 == time_out)
                        {
                            //time_count3 = time_out + 1;
                            UIMessageBox.Show("请重新测试！");
                            time_count3 = 0;
                            recvice_flag = 0;
                            recvice_data = "";
                        }
                    }


                    if (flag_warn == 0)
                    {
                        flag_warn = 1;
                        time_count3 = 0;
                        UIMessageBox.Show("已收到数据，请扫码！！！");
                        time_count3 = 0;//点击确认好清零
                    }
                }
            }
            if (time_count1 > 0) time_count1--;

            //if(time_count1 ==0)
            //{
            //    timer1.Stop();
            //    main_state = 0;
            //    uiSwitch1.Active = !Enabled;
            //}

            switch (main_state)
            {
                case 0:
                    uiLabel3.Text = "状态：等待功能打开...";
                    break;
                case 1:
                    uiLabel3.Text = "状态：请使用扫码枪扫入二维码！";
                    break;
                case 2:
                    uiLabel3.Text = "状态：正在读取串口数据，请稍后...";
                    break;
                case 3:
                    uiLabel3.Text = "状态：数据保存中，请稍后..."; ;
                    break;

                default:
                    uiLabel3.Text = "状态：等待功能打开...";
                    break;
            }



        }

        private void FPage1_Load(object sender, EventArgs e)
        {
            Row_count = 0;//初始化行数
            uiLabel3.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
            uiLabel3.Text = "状态：等待功能打开...";

            //读取ini，如果没有就创建
            string filename = Environment.CurrentDirectory + "\\Config.ini";
            string COM = IniFunc.getString("Information", "COM", null, filename);
            string BPS = IniFunc.getString("Information", "BPS", null, filename);
            if (BPS != "")
            {
                uiComboBox1.Text = COM;
                uiComboBox2.Text = BPS;





            }
            //uiDataGridView1.Columns[2].ValueType = typeof(int);

        }



        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiDataGridView1.CurrentCell = uiDataGridView1[1, 0];//选择具体单元格
            uiDataGridView1.BeginEdit(true);//处于编辑状态
        }

        public void Main_proess()//主流程
        {
            //判断是否打开
            if (uiSwitch1.Active == Enabled)
            {
                main_state = 1;
                uiDataGridView1.Rows.Add();
                uiDataGridView1.Rows[Row_count].Cells[0].Value = (Row_count + 1).ToString();
                uiDataGridView1.Rows[Row_count].Cells[1].Value = ' ';
                uiDataGridView1.Rows[Row_count].Cells[2].Value = ' ';
                uiDataGridView1.Rows[Row_count].Cells[3].Value = ' ';
                uiDataGridView1.Rows[Row_count].Cells[4].Value = ' ';
                time_count1 = time_out;
                uiDataGridView1.Rows[Row_count].ReadOnly = false;
                timer1.Start();

                time_count1 = time_out;//初始化延时
                uiDataGridView1[1, Row_count].Value = DateTime.Now;
                //选择焦点
                uiDataGridView1.CurrentCell = uiDataGridView1[2, Row_count];//选择具体单元格                
                uiDataGridView1.BeginEdit(true);//处于编辑状态
                //uiDataGridView1.EndEdit();
                timer2.Start();
            }
        }



        private void timer2_Tick(object sender, EventArgs e)
        {
            if (main_state == 2)
            {
                //校验串口信息                
                recive_proess();
            }
            else if (main_state == 1)
            {
                //获取SN码
                //             if (time_count1 > 0)
                {
                    //每秒判有没有18个
                    check_SN();
                }
            }
            else
            {
                timer2.Stop();
            }


        }

        private void FPage1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();//关闭之前关闭串口
            //读取ini，如果没有就创建
            string filename = Environment.CurrentDirectory + "\\Config.ini";
            string COM = uiComboBox1.Text;
            string BPS = uiComboBox2.Text;

            IniFunc.writeString("Information", "COM", COM, filename);
            IniFunc.writeString("Information", "BPS", BPS, filename);

        }

        public void check_SN()//判断SN是否输入玩
        {
            string str;
            //if(time_count1==1)  uiDataGridView1.CurrentCell = uiDataGridView1[3, Row_count];//选择具体单元格

            //获取当前SN
            check_cnt++;
            if(check_cnt>4)
            {
                check_cnt = 0;
                uiDataGridView1.EndEdit();
                if (uiDataGridView1[2, Row_count].Value != null)//判断不为空
                {
                    str = uiDataGridView1[2, Row_count].Value.ToString();

                    if (str.Length == 18)//如果字符大于4个
                    {
                        uiDataGridView1.Rows[Row_count].ReadOnly = true;
                        //         if (time_count1 != 0)
                        {
                            //成功读出SN，打开，读取串口数据
                            main_state = 2;
                            time_count2 = 10;//弹出超时时间
                            flag_warn = 0;//弹出标志符          

                        }
                    }
                    uiDataGridView1.BeginEdit(true);//处于不编辑状态
                }
            }
            
        }


        public void serialPort_init()
        {
            try
            {
                if (uiSwitch1.Active == Enabled)
                {
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.BaudRate = int.Parse(uiComboBox2.Text);
                        serialPort1.PortName = uiComboBox1.Text;
                        serialPort1.Open();
                    }
                }
                else
                {
                    serialPort1.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("串口不存在，打开失败，请重试！");
                uiSwitch1.Active = !Enabled;
            }

        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int a;
            try
            {
                //防止读取不全的情况 等待100ms
                Thread.Sleep(100);
                byte[] m_recvBytes = new byte[serialPort1.BytesToRead];//获取缓冲区的的字节数
                int reslut = serialPort1.Read(m_recvBytes, 0, m_recvBytes.Length);
                if (reslut <= 0)
                {
                    return;
                }
                string strResult = Encoding.ASCII.GetString(m_recvBytes, 0, m_recvBytes.Length);//转换成字符串格式的

                serialPort1.DiscardInBuffer();

                this.Invoke((EventHandler)(delegate
                {

                    richTextBox1.Text += DateTime.Now + "  " + strResult + '\n';
                    if (richTextBox1.Text.Length > 4000)
                    {
                        richTextBox1.Clear();
                    }

                    a = strResult.IndexOf("result");
                    if (a != -1)
                    {
                        if (recvice_flag == 0)
                        {
                            recvice_flag = 1;//判断result是否在最前面
                            recvice_data = strResult;
                            time_count2 = 2;
                        }
                    }


                }));
            }
            catch (Exception)
            {
                UIMessageBox.Show("串口关闭错误，请重试！");
            }
        }

        private void uiDataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void uiDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //直接弹错误框
            if ((e.RowIndex < Row_count) && (e.RowIndex != -1))//当点击的行小于当前计数行
            {
                show_error(e.RowIndex);
            }

        }

        private void uiLabel3_DoubleClick(object sender, EventArgs e)
        {
            if (richTextBox1.Visible)
            {
                richTextBox1.Visible = false;
            }
            else
            {
                richTextBox1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            UIMessageForm mf = new UIMessageForm();
            mf.MaximizeBox = true;
            mf.Size = new Size(500, 1000);
            mf.ClientSize = new Size(500, 1000);
            //mf.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            mf.ShowMessageDialog("hellow", "测试结果", false, UIStyle.Blue);
            UIMessageBox.Show("hellow", "测试结果");
            //mf.ShowMessageDialog(str1, "测试结果",false, UIStyle.Blue);
            //v = false;
            //UIMessageBox.Show(str1,"测试结果");'
            uiLabel6.Text = mf.Size.ToString();
        }

        private void uiLabel3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Visible)
            {
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                richTextBox1.ScrollToCaret();
            }

        }

        public void show_error(int row)//row：所在行，所在错误代码
        {
            string str1, str2;
            int i;
            byte a;
            //判断是否为fail
            str1 = uiDataGridView1[4, row].Value.ToString();
            if (str1 == "fail")
            {
                str1 = "选择行的错误代码：" + uiDataGridView1[4, row].Tag + "\n";
                str2 = uiDataGridView1[4, row].Tag.ToString();
                i = Convert.ToInt16(str2, 16);
                a = (byte)i;
                string[] s = new string[8];
                for (i = 0; i < 8; i++)
                {
                    if (GetBit(a, i))
                    {
                        s[i] = "pass";
                    }
                    else
                    {
                        s[i] = "fail";
                    }
                }
                str1 += "三轴检测结果:        " + s[0] + "\n";
                str1 += "温湿度检测结果:    pass" + "\n";
                str1 += "电量检测结果:        " + s[2] + "\n";
                str1 += "充电检测结果:        " + s[3] + "\n";
                str1 += "灯珠检测结果:        " + s[4] + "\n";
                str1 += "按键检测结果:        " + s[5] + "\n";
                str1 += "按键检测结果:        " + s[6] + "\n";
                str1 += "电流检测结果:        " + s[7] + "\n";

                UIMessageBox.Show(str1, "fail信息");
            }
        }

        public void recive_proess()//接受数据并处理
        {
            string str1, str2;
            int a;
            //当处于状态2时，才进行以下操作
            //if(main_state==2)
            {
                //recvice_flag = 1;
                if (recvice_flag == 1)
                {
                    recvice_flag = 0;
                    a = 0;
                    str1 = "";
                    str2 = "";

                    try
                    {
                        string[] strArray = recvice_data.Split(new char[2] { '[', ']' });
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (strArray[i].IndexOf("result") != -1)
                            {
                                a = i + 1;
                                break;
                            }
                        }
                        if (a != 0)
                        {
                            //判断result在哪
                            str1 = strArray[a];
                            string[] Array = str1.Split(',');
                            str1 = Array[0];
                            str2 = Array[1];
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据解析异常，请重试!" + ex.Message);
                        a = 0;
                        throw;
                    }

                    if (a != 0)
                    {
                        uiDataGridView1[3, Row_count].Value = str1;
                        if (str2 == "FD")
                        {
                            uiDataGridView1[4, Row_count].Value = "pass";
                            uiDataGridView1.Rows[Row_count].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            uiDataGridView1[4, Row_count].Value = "fail";
                            uiDataGridView1.Rows[Row_count].DefaultCellStyle.BackColor = Color.IndianRed;
                        }
                        uiDataGridView1[4, Row_count].Tag = str2;
                        time_count1 = time_out;
                        main_state = 3;//数据保存
                                       //直接弹错误框
                        if (uiSwitch2.Active == Enabled) show_error(Row_count);
                        txt_svae();
                    }
                    else
                    {
                        jiexi_cnt++;

                    }

                }
            }

        }

        public void txt_svae()
        {

            //继续流程
            if (uiSwitch1.Active == Enabled)
            {
                //循环主流程
                excel_count = Row_count;
                Row_count++;//行数++
                Main_proess();
                timer1.Start();
            }
            else
            {
                timer1.Stop();
            }

            //将数据存在TXT
            //如果使能，就保存数据
            string str1;

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NO.", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("SN", typeof(string));
                dt.Columns.Add("MAC", typeof(string));
                dt.Columns.Add("Reslut", typeof(string));
                DataRow d1 = dt.NewRow();

                d1[0] = uiDataGridView1.Rows[excel_count].Cells[0].Value.ToString();
                d1[1] = uiDataGridView1.Rows[excel_count].Cells[1].Value.ToString();
                d1[2] = uiDataGridView1.Rows[excel_count].Cells[2].Value.ToString();
                d1[3] = uiDataGridView1.Rows[excel_count].Cells[3].Value.ToString();
                d1[4] = uiDataGridView1.Rows[excel_count].Cells[4].Tag.ToString();
                dt.Rows.Add(d1);

                if (uiDataGridView1[4, excel_count].Value.ToString() == "fail")
                {
                    //失败记录
                    SaveCSV(dt, Environment.CurrentDirectory + "/Fail.csv");

                }
                else
                {
                    //成功记录
                    str1 = DateTime.Now.ToString("yyyy - MM - dd");
                    SaveCSV(dt, Environment.CurrentDirectory + "/PASS " + str1 + ".csv");
                }

            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.Message);
            }

        }

        private void uiButton1_Click_2(object sender, EventArgs e)
        {
            main_state = 1;
            uiDataGridView1.Rows[Row_count].ReadOnly = false;
            uiDataGridView1.Rows[Row_count].Cells[2].Value = "";
            //选择焦点
            uiDataGridView1.CurrentCell = uiDataGridView1[2, Row_count];//选择具体单元格                
            uiDataGridView1.BeginEdit(true);//处于编辑状态
            timer2.Start();
        }

        public static bool GetBit(byte b, int bitNumber)
        {
            bool bit = (b & (1 << bitNumber)) != 0;
            return bit;
        }

        public void SaveCSV(DataTable dt, string fullPath)//table数据写入csv
        {
            //FileStream fs;
            StreamWriter sw;
            int a = 0;

            try
            {
                // fs = new FileStream(fullPath, FileMode.OpenOrCreate);

                if (!File.Exists(fullPath))
                {
                    a = 1;
                }

                sw = new StreamWriter(fullPath, true, Encoding.UTF8);

                string data = "";

                if (a == 1)
                {
                    for (int i = 1; i < dt.Columns.Count; i++)//写入列名
                    {
                        data += dt.Columns[i].ColumnName.ToString();
                        if (i < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                    a = 0;
                }

                for (int i = 0; i < dt.Rows.Count; i++) //写入各行数据
                {
                    data = "";
                    for (int j = 1; j < dt.Columns.Count; j++)
                    {
                        string str = dt.Rows[i][j].ToString();
                        str = str.Replace("\"", "\"\"");//替换英文冒号 英文冒号需要换成两个冒号
                        if (str.Contains(',') || str.Contains('"')
                          || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                        {
                            str = string.Format("\"{0}\"", str);
                        }

                        data += str;
                        if (j < dt.Columns.Count - 1)
                        {
                            data += ",";
                        }
                    }
                    sw.WriteLine(data);
                }

                // close the streams
                sw.Close();
                //fs.Close();
            }
            catch (IOException ioe)
            {
                UIMessageBox.Show("保存失败！\n" + ioe.Message,false);
                try_again();
                // Console.WriteLine("IOException occurred: " + ioe.Message);
            }

            
            

        }


        public void try_again()
        {
            //将数据存在TXT
            //如果使能，就保存数据
            string str1;

            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("NO.", typeof(string));
                dt.Columns.Add("Time", typeof(string));
                dt.Columns.Add("SN", typeof(string));
                dt.Columns.Add("MAC", typeof(string));
                dt.Columns.Add("Reslut", typeof(string));
                DataRow d1 = dt.NewRow();

                d1[0] = uiDataGridView1.Rows[excel_count].Cells[0].Value.ToString();
                d1[1] = uiDataGridView1.Rows[excel_count].Cells[1].Value.ToString();
                d1[2] = uiDataGridView1.Rows[excel_count].Cells[2].Value.ToString();
                d1[3] = uiDataGridView1.Rows[excel_count].Cells[3].Value.ToString();
                d1[4] = uiDataGridView1.Rows[excel_count].Cells[4].Tag.ToString();
                dt.Rows.Add(d1);

                if (uiDataGridView1[4, excel_count].Value.ToString() == "fail")
                {
                    //失败记录
                    SaveCSV(dt, Environment.CurrentDirectory + "/Fail.csv");

                }
                else
                {
                    //成功记录
                    str1 = DateTime.Now.ToString("yyyy - MM - dd");
                    SaveCSV(dt, Environment.CurrentDirectory + "/PASS " + str1 + ".csv");
                }

            }
            catch (Exception ioe)
            {
                MessageBox.Show(ioe.Message);
            }
        }





    }
}
