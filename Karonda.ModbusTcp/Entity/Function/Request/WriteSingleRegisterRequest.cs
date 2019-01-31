using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Request
{
    public class WriteSingleRegisterRequest : WriteSingle
    {
        public WriteSingleRegisterRequest()
            : base((short)ModbusCommand.WriteSingleRegister)
        {

        }

        public WriteSingleRegisterRequest(ushort startingAddress, ushort value)
            : base((short)ModbusCommand.WriteSingleRegister, startingAddress, value)
        {
        }
    }
}
