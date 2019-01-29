using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class ReadCoilsResponse : ModbusFunction
    {
        private ushort byteCount;
        public BitArray Coils { get; private set; }
        public ReadCoilsResponse()
            : base((short)ModbusCommand.ReadCoils)
        {

        }

        public ReadCoilsResponse(BitArray coils)
            : this()
        {
            Coils = coils;
            byteCount = (ushort)(Coils.Length / 8);
        }

        public override int CalculateLength()
        {
            return 1 + byteCount;
        }

        public override void Decode(IByteBuffer buffer)
        {
            byteCount = buffer.ReadByte();
            var coils = new Byte[byteCount];
            buffer.ReadBytes(coils);

            Coils = new BitArray(coils);
        }

        public override IByteBuffer Encode()
        {
            var coils = new Byte[byteCount];
            Coils.CopyTo(coils, 0);

            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);
            buffer.WriteByte(byteCount);

            buffer.WriteBytes(coils);

            return buffer;
        }
    }
}
