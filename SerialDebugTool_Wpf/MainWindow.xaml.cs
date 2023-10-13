using SerialDebugTool_Wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
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

namespace SerialDebugTool_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            //获取所有可用串口的名称
            string[] portNames = SerialPort.GetPortNames();
            foreach (string portName in portNames)
            {
                this.cbPortNumbers.Items.Add(portName);
            }
        }

        /// <summary>
        /// 无边框鼠标拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Move_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        #region 监测串口设备插拔
        private void Test()
        {
            StartMonitoringPortChanges();

            Console.WriteLine("正在监测串口设备的插拔情况。");
            Console.WriteLine("按任意键停止监测。");

            Console.ReadKey();

            if (watcher != null)
            {
                watcher.Stop();
                watcher.Dispose();
            }
        }

        private ManagementEventWatcher? watcher;

        private void StartMonitoringPortChanges()
        {
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");

            watcher = new ManagementEventWatcher(new ManagementScope("root\\cimv2"), query);
            watcher.EventArrived += PortChangeEventReceived;
            watcher.Start();
        }

        private void PortChangeEventReceived(object sender, EventArrivedEventArgs e)
        {
            int eventType = int.Parse(e.NewEvent.Properties["EventType"].Value.ToString());

            if (eventType == 2)
            {
                Console.WriteLine("串口设备已插入.");
            }
            else if (eventType == 3)
            {
                Console.WriteLine("串口设备已拔出.");
            }

            // 你可以在这里执行其他操作，例如刷新串口列表
            string[] availablePorts = SerialPort.GetPortNames();
            Console.WriteLine("当前可用的串口:");
            foreach (string port in availablePorts)
            {
                Console.WriteLine(port);
            }
        }
        #endregion
    }
}