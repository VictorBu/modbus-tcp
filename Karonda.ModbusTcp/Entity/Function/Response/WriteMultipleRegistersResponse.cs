using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class WriteMultipleRegistersResponse : ReadWriteMultiple
    {
        public WriteMultipleRegistersResponse()
            : base((short)ModbusCommand.WriteMultipleRegisters)
        {

        }

        public WriteMultipleRegistersResponse(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.WriteMultipleRegisters, startingAddress, quantity)
        {

        }
    }
}
