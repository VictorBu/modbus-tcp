using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class ReadInputRegistersRequest : ReadWriteMultiple
    {
        public ReadInputRegistersRequest()
            : base((short)ModbusCommand.ReadInputRegisters)
        {

        }

        public ReadInputRegistersRequest(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.ReadInputRegisters, startingAddress, quantity)
        {
        }
    }
}
