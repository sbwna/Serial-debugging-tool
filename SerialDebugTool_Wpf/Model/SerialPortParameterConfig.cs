using SerialDebugTool_Wpf.Converter;
using System.IO.Ports;

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
        float stopBit;//停止位 0/1/1.5/2

        public SerialPortParameterConfig(string? portNumber, int baudRate, int dataBits, string? parityBit, float stopBit)
        {
            this.portNumber = portNumber;
            this.baudRate = baudRate;
            this.dataBits = dataBits;
            this.parityBit = parityBit;
            this.stopBit = stopBit;
        }

        public SerialPortParameterConfig()
        {

        }

        //串口
        SerialPort serialPort = new SerialPort();

        /// <summary>
        /// 打开串口
        /// </summary>
        public void Open()
        {
            serialPort = new SerialPort(this.portNumber, this.baudRate, SerialPortParameterConvertor.ParityConvertor(this.parityBit),
                this.dataBits, SerialPortParameterConvertor.StopBitsConvertor(this.stopBit));

            serialPort.Open();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData"></param>
        public void Send(string sendData)
        {
            serialPort.WriteLine(sendData);
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void Close()
        {
            serialPort.Close();
        }
    }


}