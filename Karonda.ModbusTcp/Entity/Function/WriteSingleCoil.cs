using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function
{
    public class WriteSingleCoil : WriteSingle
    {
        public bool State
        {
            get
            {
                return Value == 0xFF00;
            }
        }
        public WriteSingleCoil()
            : base((short)ModbusCommand.WriteSingleCoil)
        {

        }

        public WriteSingleCoil(ushort startingAddress, bool state)
            : base((short)ModbusCommand.WriteSingleCoil, startingAddress, (ushort)(state ? 0xFF00 : 0x0000))
        {
        }
    }
}
