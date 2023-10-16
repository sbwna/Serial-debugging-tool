using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SerialDebugTool_Wpf.Converter;
using System.IO.Ports;
using System.Windows.Documents;
using MessageBox = HandyControl.Controls.MessageBox;

namespace SerialDebugTool_Wpf.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            ReceviedData = new FlowDocument(new Paragraph(new Run("")));
        }

        //串口
        private static SerialPort serialPort = new SerialPort();

        #region 属性
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
        private FlowDocument receviedData;
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

                serialPort.Open();
                serialPort.DataReceived += SerialPort_DataReceived;
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
            //Paragraph p = new Paragraph(new Run(serialPort.ReadExisting()));
            //ReceviedData.Blocks.Add(p);

            MessageBox.Info(serialPort.ReadExisting(), "数据已接收");
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

            serialPort.WriteLine(SendData);
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