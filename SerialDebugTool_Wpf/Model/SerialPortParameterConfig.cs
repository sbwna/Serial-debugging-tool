using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDebugTool_Wpf.Model
{
    /// <summary>
    /// 串口参数配置
    /// </summary>
    public class SerialPortParameterConfig
    {
        string? portNumber;//端口号 COM1。。。
        int baudRate;//波特率 110/300/600/1200/2400/4800/9600/14400/19200/38400/56000/57600/115200
        int dataBits;//数据位 6/7/8
        string? parityBit;//校验位 None/Odd/Even/Mark/Space
        float stopBit;//停止位 1/1.5/2

        bool isOpen;//状态 true=>Open；false=>Close

        public SerialPortParameterConfig(string? portNumber, int baudRate, int dataBits, string? parityBit, float stopBit, bool isOpen)
        {
            this.portNumber = portNumber;
            this.baudRate = baudRate;
            this.dataBits = dataBits;
            this.parityBit = parityBit;
            this.stopBit = stopBit;
            this.isOpen = isOpen;
        }
    }
}
