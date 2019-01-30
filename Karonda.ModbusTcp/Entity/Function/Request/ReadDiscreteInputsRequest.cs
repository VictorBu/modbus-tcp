using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class ReadDiscreteInputsRequest : ReadWriteMultiple
    {
        public ReadDiscreteInputsRequest()
            : base((short)ModbusCommand.ReadDiscreteInputs)
        {

        }

        public ReadDiscreteInputsRequest(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.ReadDiscreteInputs, startingAddress, quantity)
        {
        }
    }
}
