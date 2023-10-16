using System.IO.Ports;

namespace SerialDebugTool_Wpf.Converter
{
    /// <summary>
    /// 串口配置参数转换器
    /// </summary>
    public static class SerialPortParameterConvertor
    {
        public static Parity ParityConvertor(string parityBit)
        {
            Parity result = new Parity();

            switch (parityBit)
            {
                case "None":
                    result = Parity.None;
                    break;
                case "Odd":
                    result = Parity.Odd;
                    break;
                case "Even":
                    result = Parity.Even;
                    break;
                case "Mark":
                    result = Parity.Mark;
                    break;
                case "Space":
                    result = Parity.Space;
                    break;
            }

            return result;
        }

        public static StopBits StopBitsConvertor(float stopBits)
        {
            StopBits result = new StopBits();

            switch (stopBits)
            {
                case 0:
                    result = StopBits.None;
                    break;
                case 1:
                    result = StopBits.One;
                    break;
                case 1.5f:
                    result = StopBits.OnePointFive;
                    break;
                case 2:
                    result = StopBits.Two;
                    break;
            }

            return result;
        }
    }
}
