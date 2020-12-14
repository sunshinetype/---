using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;

namespace 主窗口
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static bool _continue;
        public SerialPort _serialPort = new SerialPort();
        List<combobox_list> 波特率列表 = new List<combobox_list>();
        List<combobox_list> 校验位列表 = new List<combobox_list>();
        List<combobox_list> 数据位列表 = new List<combobox_list>();
        List<combobox_list> 校验方式列表 = new List<combobox_list>();
        public MainWindow()
        {
            InitializeComponent();


        }
        public class combobox_list
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }
        private void 端口选择_combobox_MouseEnter(object sender, MouseEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            端口选择_combobox.Items.Clear();
            for (int index = 0; index < ports.Length; index++)
            {

                端口选择_combobox.Items.Add(ports[index]);//添加item
                端口选择_combobox.SelectedIndex = index; //设置显示的item索引
            }
        }

        private void ___主窗口__Initialized(object sender, EventArgs e)
        {

            波特率列表.Add(new combobox_list { Name = "600", ID = 1 });
            波特率列表.Add(new combobox_list { Name = "1200", ID = 2 });
            波特率列表.Add(new combobox_list { Name = "2400", ID = 3 });
            波特率列表.Add(new combobox_list { Name = "4800", ID = 4 });
            波特率列表.Add(new combobox_list { Name = "9600", ID = 5 });
            波特率列表.Add(new combobox_list { Name = "14400", ID = 6 });
            波特率列表.Add(new combobox_list { Name = "19200", ID = 7 });
            波特率列表.Add(new combobox_list { Name = "28800", ID = 8 });
            波特率列表.Add(new combobox_list { Name = "38400", ID = 9 });
            波特率列表.Add(new combobox_list { Name = "57600", ID = 10 });
            波特率列表.Add(new combobox_list { Name = "115200", ID = 11 });
            波特率列表.Add(new combobox_list { Name = "230400", ID = 12 });
            波特率列表.Add(new combobox_list { Name = "460800", ID = 13 });
            波特率_combobox.ItemsSource = 波特率列表;
            波特率_combobox.DisplayMemberPath = "Name";
            波特率_combobox.SelectedValuePath = "ID";
            波特率_combobox.SelectedValue = 5;



            校验位列表.Add(new combobox_list { Name = "1", ID = 1 });
            校验位列表.Add(new combobox_list { Name = "1.5", ID = 2 });
            校验位列表.Add(new combobox_list { Name = "2", ID = 3 });
            停止位_combobox.ItemsSource = 校验位列表;
            停止位_combobox.DisplayMemberPath = "Name";
            停止位_combobox.SelectedValuePath = "ID";
            停止位_combobox.SelectedValue = 1;



            数据位列表.Add(new combobox_list { Name = "8", ID = 1 });
            数据位列表.Add(new combobox_list { Name = "7", ID = 2 });
            数据位列表.Add(new combobox_list { Name = "6", ID = 3 });
            数据位列表.Add(new combobox_list { Name = "5", ID = 4 });
            数据位列表.Add(new combobox_list { Name = "4", ID = 5 });
            数据位_combobox.ItemsSource = 数据位列表;
            数据位_combobox.DisplayMemberPath = "Name";
            数据位_combobox.SelectedValuePath = "ID";
            数据位_combobox.SelectedValue = 1;



            校验方式列表.Add(new combobox_list { Name = "无", ID = 1 });
            校验方式列表.Add(new combobox_list { Name = "奇校验", ID = 2 });
            校验方式列表.Add(new combobox_list { Name = "偶校验", ID = 3 });
            校验方式_combobox.ItemsSource = 校验方式列表;
            校验方式_combobox.DisplayMemberPath = "Name";
            校验方式_combobox.SelectedValuePath = "ID";
            校验方式_combobox.SelectedValue = 1;

        }

        private void 串口状态_Click(object sender, RoutedEventArgs e)
        {
            string strContent = this.串口状态.Content.ToString();
            if ((端口选择_combobox.SelectedValue != null) & (strContent == "开启端口"))
            {


                _serialPort.PortName = 端口选择_combobox.SelectedItem.ToString();//串口号
                _serialPort.BaudRate = Convert.ToInt32(波特率_combobox.Text);//波特率
                //显示.Text = _serialPort.BaudRate.ToString();
                _serialPort.DataBits = Convert.ToInt32(数据位_combobox.Text); ;//数据位
                switch (停止位_combobox.SelectedValue)
                {
                    case 1:
                        _serialPort.StopBits = StopBits.One;
                        break;
                    case 2:
                        _serialPort.StopBits = StopBits.OnePointFive;
                        break;
                    case 3:
                        _serialPort.StopBits = StopBits.Two;
                        break;
                }

                switch (校验方式_combobox.SelectedValue)
                {
                    case 1:
                        _serialPort.Parity = Parity.None;
                        break;
                    case 2:
                        _serialPort.Parity = Parity.Odd;
                        break;
                    case 3:
                        _serialPort.Parity = Parity.Even;
                        break;
                }

                _serialPort.Open();
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);//添加数据接收事件
                //_serialPort.DataReceived += DataReceivedHandler;
                串口状态.Content = "端口已开启";

            }
            else if (strContent == "端口已开启")
            {
                // _serialPort.DataReceived -= DataReceivedHandler;

                _serialPort.Close();
                串口状态.Content = "开启端口";
            }
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)//读取下位机的数据，显示在textBlock中
        {
            int len = this._serialPort.BytesToRead;
            byte[] buffer = new byte[len];
            this._serialPort.Read(buffer, 0, len);
            string strData = BitConverter.ToString(buffer, 0, len);
            Dispatcher.Invoke(() =>
            {
                this.接收显示.Text += strData;
               this.接收显示.Text += "-";
            });

            
        }

        private void 发送数据_Click(object sender, RoutedEventArgs e)
        {
            string SendData = 发送显示.Text;
            byte[] Data = new byte[SendData.Length];
            for (int i = 0; i < SendData.Length / 2; i++)
            {
                //每次取两位字符组成一个16进制
                // Data[i] = Convert.ToByte(发送显示.Text.Substring(i * 2, 2), 16);
                
            }
            // Data[0] = Convert.ToByte(发送显示.Text, 16);
            // this._serialPort.Write(Data, 0, Data.Length);
            this._serialPort.Write(SendData);
        }

    
    }
}


