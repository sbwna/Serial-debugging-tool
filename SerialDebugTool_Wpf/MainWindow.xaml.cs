using SerialDebugTool_Wpf.ViewModel;
using System.Windows;
using System.Windows.Input;

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

            var mwvm = new MainWindowViewModel();
            this.DataContext = mwvm;

            //初始化配置
            InitConfig();

            this.rtbReceviedData.Document = mwvm.ReceviedData;
        }

        /// <summary>
        /// 配置初始化
        /// </summary>
        private void InitConfig()
        {
            int[] baudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 230400, 460800, 921600 };
            int[] dataBits = { 6, 7, 8 };
            string[] parityBits = { "None", "Odd", "Even", "Mark", "Space" };
            float[] stopBits = { 0, 1, 1.5f, 2 };

            foreach (var br in baudRates)
            {
                this.cbBaudRate.Items.Add(br);
            }

            foreach (var db in dataBits)
            {
                this.cbDataBits.Items.Add(db);
            }

            foreach (var pb in parityBits)
            {
                this.cbParityBit.Items.Add(pb);
            }

            foreach (var sb in stopBits)
            {
                this.cbStopBit.Items.Add(sb);
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
    }
}