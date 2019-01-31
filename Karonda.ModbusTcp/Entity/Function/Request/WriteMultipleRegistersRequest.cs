using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class WriteMultipleRegistersRequest : ReadWriteMultiple
    {
        private ushort byteCount;
        public ushort[] Registers { get; private set; }
        public WriteMultipleRegistersRequest()
            : base((short)ModbusCommand.WriteMultipleRegisters)
        {

        }

        public WriteMultipleRegistersRequest(ushort startingAddress, ushort[] registers)
            : base((short)ModbusCommand.WriteMultipleRegisters, startingAddress, (ushort)registers.Length)
        {
            byteCount = (ushort)(Quantity * 2);
            Registers = registers;
        }

        public override int CalculateLength()
        {
            return base.CalculateLength() + 1 + byteCount;
        }

        public override void Decode(IByteBuffer buffer)
        {
            base.Decode(buffer);

            byteCount = buffer.ReadByte();

            Registers = new ushort[byteCount / 2];
            for(int i=0;i<Registers.Length;i++)
            {
                Registers[i] = buffer.ReadUnsignedShort();
            }
        }

        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = base.Encode();

            buffer.WriteByte(byteCount);

            foreach (var register in Registers)
            {
                buffer.WriteUnsignedShort(register);
            }

            return buffer;
        }
    }
}
