using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * Modbus TCP Frame Description
 *  - max. 260 Byte (ADU = 7 Byte MBAP + 253 Byte PDU)
 *  - Length field includes Unit Identifier + PDU
 *
 * <----------------------------------------------- ADU -------------------------------------------------------->
 * <---------------------------- MBAP -----------------------------------------><------------- PDU ------------->
 * +------------------------+---------------------+----------+-----------------++---------------+---------------+
 * | Transaction Identifier | Protocol Identifier | Length   | Unit Identifier || Function Code | Data          |
 * | (2 Byte)               | (2 Byte)            | (2 Byte) | (1 Byte)        || (1 Byte)      | (1 - 252 Byte |
 * +------------------------+---------------------+----------+-----------------++---------------+---------------+
 */

namespace Karonda.ModbusTcp.Entity
{
    public class ModbusFrame
    {
        public ModbusHeader Header { get; set; }
        public ModbusFunction Function { get; set; }

        public ModbusFrame(ModbusHeader header, ModbusFunction function)
        {
            Header = header;
            Function = function;
        }

        public IByteBuffer Encode()
        {
            Header.Length = (ushort)(1 + 1 + Function.CalculateLength());// Unit Identifier + Function Code + data length

            IByteBuffer buffer = Unpooled.Buffer();

            buffer.WriteBytes(Header.Encode());
            buffer.WriteBytes(Function.Encode());

            return buffer;
        }
    }
}
