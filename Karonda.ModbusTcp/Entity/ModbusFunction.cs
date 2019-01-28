using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity
{
    public abstract class ModbusFunction
    {
        protected short FunctionCode { get; }

        protected ModbusFunction(short functionCode)
        {
            FunctionCode = functionCode;
        }
        /// <summary>
        /// PDU length -1 (not include function code length)
        /// </summary>
        /// <returns></returns>
        public abstract int CalculateLength();   

        public abstract void Decode(IByteBuffer buffer);

        public abstract IByteBuffer Encode();

    }
}
