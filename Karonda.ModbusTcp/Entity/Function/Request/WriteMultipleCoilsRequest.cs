using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class WriteMultipleCoilsRequest : ReadWriteMultiple
    {
        private ushort byteCount;
        public BitArray Coils { get; private set; }
        public WriteMultipleCoilsRequest()
            : base((short)ModbusCommand.WriteMultipleCoils)
        {

        }

        public WriteMultipleCoilsRequest(ushort startingAddress, bool[] states)
            : base((short)ModbusCommand.WriteMultipleCoils, startingAddress, (ushort)states.Length)
        {
            var length = Quantity + (8 - Quantity % 8) % 8;

            if(length > Quantity)
            {
                var finalCoils = new List<bool>();
                finalCoils.AddRange(states);
                for(var i = Quantity; i < length; i++)
                {
                    finalCoils.Add(false);
                }

                Coils = new BitArray(finalCoils.ToArray());
            }
            else
            {
                Coils = new BitArray(states);
            }

            byteCount = (ushort)(Coils.Length / 8);
        }

        public override int CalculateLength()
        {
            return base.CalculateLength() + 1 + byteCount;
        }

        public override void Decode(IByteBuffer buffer)
        {
            base.Decode(buffer);

            byteCount = buffer.ReadByte();
            var coils = new byte[byteCount];
            buffer.ReadBytes(coils);

            Coils = new BitArray(coils);
        }

        public override IByteBuffer Encode()
        {
            var coils = new byte[byteCount];
            Coils.CopyTo(coils, 0);

            IByteBuffer buffer = base.Encode();

            buffer.WriteByte(byteCount);

            buffer.WriteBytes(coils);

            return buffer;
        }
    }
}
