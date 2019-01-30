using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class ReadCoilsInputsResponse : ModbusFunction
    {
        private ushort byteCount;
        protected BitArray CoilsOrInputs { get; private set; }
        public ReadCoilsInputsResponse(short functionCode)
            : base(functionCode)
        {

        }

        public ReadCoilsInputsResponse(short functionCode, BitArray coilsOrInputs)
            : this(functionCode)
        {
            CoilsOrInputs = coilsOrInputs;
            byteCount = (ushort)(CoilsOrInputs.Length / 8);
        }

        public override int CalculateLength()
        {
            return 1 + byteCount;
        }

        public override void Decode(IByteBuffer buffer)
        {
            byteCount = buffer.ReadByte();
            var coilsOrInputs = new Byte[byteCount];
            buffer.ReadBytes(coilsOrInputs);

            CoilsOrInputs = new BitArray(coilsOrInputs);
        }

        public override IByteBuffer Encode()
        {
            var coilsOrInputs = new Byte[byteCount];
            CoilsOrInputs.CopyTo(coilsOrInputs, 0);

            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);
            buffer.WriteByte(byteCount);

            buffer.WriteBytes(coilsOrInputs);

            return buffer;
        }
    }
}
