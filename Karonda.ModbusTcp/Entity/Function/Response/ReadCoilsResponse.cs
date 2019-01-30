using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Entity.Function.Response
{
    public class ReadCoilsResponse : ReadCoilsInputsResponse
    {
        public BitArray Coils
        {
            get
            {
                return CoilsOrInputs;
            }
        }

        public ReadCoilsResponse()
            : base((short)ModbusCommand.ReadCoils)
        {

        }

        public ReadCoilsResponse(BitArray coils)
            : base((short)ModbusCommand.ReadCoils, coils)
        {
        }
    }
}
