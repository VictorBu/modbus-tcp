using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity
{
    public class ModbusHeader
    {
        public ushort TransactionIdentifier { get; set; }
        public ushort ProtocolIdentifier { get; set; }
        public ushort Length { get; set; }
        public short UnitIdentifier { get; set; }

        public ModbusHeader(IByteBuffer buffer)
        {
            TransactionIdentifier = buffer.ReadUnsignedShort();
            ProtocolIdentifier = buffer.ReadUnsignedShort();
            Length = buffer.ReadUnsignedShort();
            UnitIdentifier = buffer.ReadByte(); // readUnsignedByte
        }

        public ModbusHeader(ushort transactionIdentifier, short unitIdentifier)
            : this(transactionIdentifier, 0x0000, unitIdentifier) // for modbus protocol: Protocol Identifier = 0x00
        {

        }

        private ModbusHeader(ushort transactionIdentifier, ushort protocolIdentifier, short unitIdentifier)
        {
            TransactionIdentifier = transactionIdentifier;
            ProtocolIdentifier = protocolIdentifier;
            UnitIdentifier = unitIdentifier;
        }

        public IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();

            buffer.WriteUnsignedShort(TransactionIdentifier);
            buffer.WriteUnsignedShort(ProtocolIdentifier);
            buffer.WriteUnsignedShort(Length);
            buffer.WriteByte(UnitIdentifier);

            return buffer;
        }
    }
}
