using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class AddressQuantityBase : ModbusFunction
    {
        public ushort StartingAddress { get; protected set; }
        public ushort Quantity { get; protected set; }

        public AddressQuantityBase(short functionCode)
            : base(functionCode)
        {
        }

        public AddressQuantityBase(short functionCode, ushort startingAddress, ushort quantity)
            : base(functionCode)
        {
            StartingAddress = startingAddress;
            Quantity = quantity;
        }

        public override int CalculateLength()
        {
            return 2 + 2; // StartingAddress Length + Quantity Length
        }
        public override void Decode(IByteBuffer buffer)
        {
            StartingAddress = buffer.ReadUnsignedShort();
            Quantity = buffer.ReadUnsignedShort();
        }
        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);

            buffer.WriteUnsignedShort(StartingAddress);
            buffer.WriteUnsignedShort(Quantity);

            return buffer;
        }
    }
}
