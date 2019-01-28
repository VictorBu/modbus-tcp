using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class ReadHoldingRegistersResponse : ModbusFunction
    {
        private ushort byteCount;
        public ushort[] Registers { get; private set; }
        public ReadHoldingRegistersResponse()
            : base((short)ModbusCommand.ReadHoldingRegisters)
        {

        }

        public ReadHoldingRegistersResponse(ushort[] registers)
            : this()
        {
            Registers = registers;
            byteCount = (ushort)(registers.Length * 2);
        }

        public override int CalculateLength()
        {
            return 1 + byteCount;
        }

        public override void Decode(IByteBuffer buffer)
        {
            byteCount = buffer.ReadByte();
            Registers = new ushort[byteCount/2];
            for (int i = 0; i < Registers.Length; i++)
            {
                Registers[i] = buffer.ReadUnsignedShort();
            }

        }

        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);
            buffer.WriteByte(byteCount);

            foreach(var register in Registers)
            {
                buffer.WriteUnsignedShort(register);
            }

            return buffer;
        }
    }
}
