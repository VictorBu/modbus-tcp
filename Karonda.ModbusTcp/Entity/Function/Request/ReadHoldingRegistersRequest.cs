using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class ReadHoldingRegistersRequest : AddressQuantityBase
    {
        public ReadHoldingRegistersRequest()
            : base((short)ModbusCommand.ReadHoldingRegisters)
        {

        }

        public ReadHoldingRegistersRequest(ushort startingAddress, ushort quantity)
            : base((short)ModbusCommand.ReadHoldingRegisters, startingAddress, quantity)
        {
        }
    }
}
