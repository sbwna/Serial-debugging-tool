using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SerialDebugTool_Wpf.Converter;
using System;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SerialDebugTool_Wpf.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        #region 属性
        //串口
        private static SerialPort serialPort = new SerialPort();

        /// <summary>
        /// 当前串口号
        /// </summary>
        private string? portNumber;
        public string? PortNumber
        {
            get => portNumber;
            set => SetProperty(ref portNumber, value);
        }

        /// <summary>
        /// 串口列表
        /// </summary>
        private string[]? portNumLst;
        public string[]? PortNumLst
        {
            get => portNumLst;
            set => SetProperty(ref portNumLst, value);
        }

        /// <summary>
        /// 波特率
        /// </summary>
        private int baudRate;
        public int BaudRate
        {
            get => baudRate;
            set => SetProperty(ref baudRate, value);
        }

        /// <summary>
        /// 数据位
        /// </summary>
        private int dataBits;
        public int DataBits
        {
            get => dataBits;
            set => SetProperty(ref dataBits, value);
        }

        /// <summary>
        /// 校验位
        /// </summary>
        private string? parityBit;
        public string? ParityBit
        {
            get => parityBit;
            set => SetProperty(ref parityBit, value);
        }

        /// <summary>
        /// 数据位
        /// </summary>
        private float stopBit;
        public float StopBit
        {
            get => stopBit;
            set => SetProperty(ref stopBit, value);
        }

        /// <summary>
        /// 状态
        /// </summary>
        private bool isOpen = false;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }

        /// <summary>
        /// 接收区数据
        /// </summary>
        private FlowDocument receviedData = new FlowDocument();
        public FlowDocument ReceviedData
        {
            get => receviedData;
            set => SetProperty(ref receviedData, value);
        }

        /// <summary>
        /// 发送区数据
        /// </summary>
        private string sendData;
        public string SendData
        {
            get => sendData;
            set => SetProperty(ref sendData, value);
        }

        /// <summary>
        /// 打开串口按钮内容
        /// </summary>
        private string openPortButtonContent = "打开串口";
        public string OpenPortButtonContent
        {
            get => openPortButtonContent;
            set => SetProperty(ref openPortButtonContent, value);
        }

        /// <summary>
        /// 发送数据格式
        /// </summary>
        private bool sendDataFormat = true;
        public bool SendDataFormat
        {
            get => sendDataFormat;
            set => SetProperty(ref sendDataFormat, value);
        }

        /// <summary>
        /// 接收数据格式
        /// </summary>
        private bool receivedDataFormat = true;
        public bool ReceivedDataFormat
        {
            get => receivedDataFormat;
            set => SetProperty(ref receivedDataFormat, value);
        }
        #endregion

        #region 命令
        /// <summary>
        /// 获取串口列表
        /// </summary>
        private RelayCommand? getPortNumLst;
        public IRelayCommand GetPortNumLstCommand => getPortNumLst ??= new RelayCommand(GetPortNumLst);

        private void GetPortNumLst()
        {
            //获取所有可用串口的名称
            string[] portNames = SerialPort.GetPortNames();

            PortNumLst = portNames;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        private RelayCommand? openSerialPort;
        public IRelayCommand OpenSerialPortCommand => openSerialPort ??= new RelayCommand(OpenSerialPort);

        private void OpenSerialPort()
        {
            // 配置为空
            if (PortNumber == null || BaudRate == null || DataBits == null || ParityBit == null || StopBit == null)
            {
                MessageBox.Error("串口参数未配置", "打开串口失败");
                return;
            }

            IsOpen = !IsOpen;
            if (IsOpen)
            {
                OpenPortButtonContent = "关闭串口";
                serialPort = new SerialPort(PortNumber, BaudRate, SerialPortParameterConvertor.ParityConvertor(ParityBit),
                    DataBits, SerialPortParameterConvertor.StopBitsConvertor(StopBit));

                try
                {
                    serialPort.Open();
                    serialPort.DataReceived += SerialPort_DataReceived;
                }
                catch
                {
                    MessageBox.Error("串口参数配置错误", "打开串口失败");
                }
            }
            else
            {
                OpenPortButtonContent = "打开串口";
                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.Close();
            }
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = "";

            //读取数据
            int bytesToRead = serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToRead];
            serialPort.Read(buffer, 0, bytesToRead);

            //ASCII格式
            if (ReceivedDataFormat)
            {
                data = System.Text.Encoding.ASCII.GetString(buffer);
            }
            //HEX格式
            else
            {
                data = BitConverter.ToString(buffer);
            }

            // 在需要时使用 Dispatcher 返回 UI 线程
            Application.Current.Dispatcher.Invoke(() =>
            {
                // 更新 UI 元素
                string receviedHead = $"[{DateTime.Now.ToString()}]收◄◄◄" + data;
                Paragraph p = new Paragraph(new Run(receviedHead));
                ReceviedData.Blocks.Add(p);
            });
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        private RelayCommand? send;
        public IRelayCommand SendDataCommand => send ??= new RelayCommand(Send);

        private void Send()
        {
            if (!IsOpen)
            {
                MessageBox.Error("串口未打开", "打开串口失败");
                return;
            }

            //ASCII格式
            if (SendDataFormat)
            {
                try
                {
                    serialPort.WriteLine(SendData);
                }
                catch
                {
                    MessageBox.Error("数据发送失败", "串口通信");
                }
            }
            //HEX
            else
            {
                //HEX格式：0~9、a~f任意组合
                if (!IsValidHex(SendData))
                {
                    MessageBox.Error("输入的不是有效HEX字符组合！（有效格式为：只能包含0~9、A~F、a~f，且不能包含空格。）", "发送失败");
                    return;
                }

                try
                {
                    byte[] byteData = HexStringToByteArray(SendData);
                    serialPort.Write(byteData, 0, byteData.Length);
                }
                catch
                {
                    MessageBox.Error("数据发送失败", "串口通信");
                }
            }

            // 更新 UI 元素
            Application.Current.Dispatcher.Invoke(() =>
            {
                string sendHead = $"[{DateTime.Now.ToString()}]发►►►" + SendData;
                Paragraph p = new Paragraph(new Run(sendHead));
                ReceviedData.Blocks.Add(p);
            });
        }

        /// <summary>
        /// 检查是否满足HEX字符格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsValidHex(string input)
        {
            // 使用正则表达式检查输入是否合法
            string pattern = @"^[0-9A-Fa-f\s]+$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        private byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length / 2;
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 清空接收数据
        /// </summary>
        private RelayCommand? clearReceviedData;
        public IRelayCommand ClearReceviedDataCommand => clearReceviedData ??= new RelayCommand(ClearReceviedData);

        private void ClearReceviedData()
        {
            ReceviedData.Blocks.Clear();
        }

        /// <summary>
        /// 清空发送数据
        /// </summary>
        private RelayCommand? clearSendData;
        public IRelayCommand ClearSendDataCommand => clearSendData ??= new RelayCommand(ClearSendData);

        private void ClearSendData()
        {
            SendData = string.Empty;
        }

        /// <summary>
        /// 程序退出
        /// </summary>
        private RelayCommand? exit;
        public IRelayCommand ExitCommand => exit ??= new RelayCommand(Exit);

        private void Exit()
        {
            System.Environment.Exit(0);
        }

        #endregion
    }
}