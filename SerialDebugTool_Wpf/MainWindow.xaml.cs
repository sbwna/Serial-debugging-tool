using SerialDebugTool_Wpf.ViewModel;
using System;
using System.IO.Ports;
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

            this.DataContext = new MainWindowViewModel();
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