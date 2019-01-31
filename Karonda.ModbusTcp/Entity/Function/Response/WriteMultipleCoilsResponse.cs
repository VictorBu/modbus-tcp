using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class WriteMultipleCoilsResponse : ReadWriteMultiple
    {
        public WriteMultipleCoilsResponse()
            : base((short)ModbusCommand.WriteMultipleCoils)
        {

        }

        public WriteMultipleCoilsResponse(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.WriteMultipleCoils, startingAddress, quantity)
        {

        }
    }
}
