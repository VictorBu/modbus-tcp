using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class ReadCoilsRequest : ReadWriteMultiple
    {
        public ReadCoilsRequest()
            : base((short)ModbusCommand.ReadCoils)
        {

        }

        public ReadCoilsRequest(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.ReadCoils, startingAddress, quantity)
        {
        }
    }
}
