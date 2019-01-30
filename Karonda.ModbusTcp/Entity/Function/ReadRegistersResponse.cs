using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function
{
    public abstract class ReadRegistersResponse : ModbusFunction
    {
        private ushort byteCount;
        public ushort[] Registers { get; private set; }
        public ReadRegistersResponse(short functionCode)
            : base(functionCode)
        {

        }

        public ReadRegistersResponse(short functionCode, ushort[] registers)
            : this(functionCode)
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
            Registers = new ushort[byteCount / 2];
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

            foreach (var register in Registers)
            {
                buffer.WriteUnsignedShort(register);
            }

            return buffer;
        }
    }
}
