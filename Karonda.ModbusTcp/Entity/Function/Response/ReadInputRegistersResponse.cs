using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class ReadInputRegistersResponse : ReadRegistersResponse
    {
        public ReadInputRegistersResponse()
            : base((short)ModbusCommand.ReadInputRegisters)
        {

        }

        public ReadInputRegistersResponse(ushort[] registers)
            : base((short)ModbusCommand.ReadInputRegisters, registers)
        {
        }
    }
}
