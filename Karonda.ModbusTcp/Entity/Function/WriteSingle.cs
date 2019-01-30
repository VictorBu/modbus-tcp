using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class WriteSingle : ModbusFunction
    {
        public ushort StartingAddress { get; private set; }
        public ushort Value { get; private set; }

        public WriteSingle(short functionCode)
            : base(functionCode)
        {

        }

        public WriteSingle(short functionCode, ushort startingAddress, ushort value)
            : base(functionCode)
        {
            StartingAddress = startingAddress;
            Value = value;
        }

        public override int CalculateLength()
        {
            return 2 + 2; // StartingAddress Length + Value Length
        }
        public override void Decode(IByteBuffer buffer)
        {
            StartingAddress = buffer.ReadUnsignedShort();
            Value = buffer.ReadUnsignedShort();
        }
        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);

            buffer.WriteUnsignedShort(StartingAddress);
            buffer.WriteUnsignedShort(Value);

            return buffer;
        }
    }
}
