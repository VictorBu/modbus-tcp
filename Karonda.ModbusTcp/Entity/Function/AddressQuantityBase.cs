using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class AddressQuantityBase : ModbusFunction
    {
        protected ushort startingAddress;
        protected ushort quantity;

        public AddressQuantityBase(short functionCode)
            : base(functionCode)
        {
        }

        public AddressQuantityBase(short functionCode, ushort startingAddress, ushort quantity)
            : base(functionCode)
        {
            this.startingAddress = startingAddress;
            this.quantity = quantity;
        }

        public override int CalculateLength()
        {
            return 2 + 2; // StartingAddress Length + Quantity Length
        }
        public override void Decode(IByteBuffer buffer)
        {
            startingAddress = buffer.ReadUnsignedShort();
            quantity = buffer.ReadUnsignedShort();
        }
        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);

            buffer.WriteUnsignedShort(startingAddress);
            buffer.WriteUnsignedShort(quantity);

            return buffer;
        }
    }
}
