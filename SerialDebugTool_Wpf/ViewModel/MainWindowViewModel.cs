using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO.Ports;

namespace SerialDebugTool_Wpf.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
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
            IsOpen = !IsOpen;
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