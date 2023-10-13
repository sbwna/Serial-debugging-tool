using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SerialDebugTool_Wpf.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private int? number;
        public int? Number
        {
            get => number;
            set => SetProperty(ref number, value);
        }

        private RelayCommand? getNumbers;
        public IRelayCommand GetNumbersCommand => getNumbers ??= new RelayCommand(GetNumber);

        private void GetNumber()
        {

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