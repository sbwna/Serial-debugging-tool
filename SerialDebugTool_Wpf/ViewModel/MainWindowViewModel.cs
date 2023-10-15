using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO.Ports;

namespace SerialDebugTool_Wpf.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
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
        /// 程序退出
        /// </summary>
        private RelayCommand? close;
        public IRelayCommand CloseCommand => close ??= new RelayCommand(Close);

        private void Close()
        {
            System.Environment.Exit(0);
        }
    }
}