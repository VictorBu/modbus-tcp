using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class WriteSingleRegisterResponse : WriteSingle
    {
        public WriteSingleRegisterResponse()
            : base((short)ModbusCommand.WriteSingleRegister)
        {

        }

        public WriteSingleRegisterResponse(ushort startingAddress, ushort value)
            : base((short)ModbusCommand.WriteSingleRegister, startingAddress, value)
        {
        }
    }
}
