using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class ReadWriteMultiple : ModbusFunction
    {
        private readonly ushort MaxReadCoilsQuantity = 0x07D0;// 2000
        private readonly ushort MaxWriteCoilsQuantity = 0x07B0;// 1968
        private readonly ushort MaxReadRegistersQuantity = 0x007D;// 125
        private readonly ushort MaxWriteRegistersQuantity = 0x007B;//123
        private readonly string QuantityOutOfRange = "quantities must no more than {0}";

        public ushort StartingAddress { get; private set; }
        public ushort Quantity { get; private set; }

        public ReadWriteMultiple(short functionCode)
            : base(functionCode)
        {
        }

        public ReadWriteMultiple(short functionCode, ushort startingAddress, ushort quantity)
            : base(functionCode)
        {
            StartingAddress = startingAddress;
            Quantity = quantity;

            WhetherQuantityIsOutOfRange();
        }

        public override int CalculateLength()
        {
            return 2 + 2; // StartingAddress Length + Quantity Length
        }
        public override void Decode(IByteBuffer buffer)
        {
            StartingAddress = buffer.ReadUnsignedShort();
            Quantity = buffer.ReadUnsignedShort();

            WhetherQuantityIsOutOfRange();
        }
        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);

            buffer.WriteUnsignedShort(StartingAddress);
            buffer.WriteUnsignedShort(Quantity);

            return buffer;
        }

        private void WhetherQuantityIsOutOfRange()
        {
            var exceptionMessage = string.Empty;
            switch ((ModbusCommand)FunctionCode)
            {
                case ModbusCommand.ReadCoils:
                case ModbusCommand.ReadDiscreteInputs:
                    if (Quantity > MaxReadCoilsQuantity)
                        exceptionMessage = string.Format(QuantityOutOfRange, MaxReadCoilsQuantity);
                    break;
                case ModbusCommand.ReadHoldingRegisters:
                case ModbusCommand.ReadInputRegisters:
                    if (Quantity > MaxReadRegistersQuantity)
                        exceptionMessage = string.Format(QuantityOutOfRange, MaxReadRegistersQuantity);
                    break;
                case ModbusCommand.WriteMultipleCoils:
                    if (Quantity > MaxWriteCoilsQuantity)
                        exceptionMessage =  string.Format(QuantityOutOfRange, MaxWriteCoilsQuantity);
                    break;
                case ModbusCommand.WriteMultipleRegisters:
                    if (Quantity > MaxWriteRegistersQuantity)
                        exceptionMessage =  string.Format(QuantityOutOfRange, MaxWriteRegistersQuantity);
                    break;
            }

            if (!string.IsNullOrEmpty(exceptionMessage))
                throw new ArgumentOutOfRangeException(exceptionMessage);
        }
    }
}
