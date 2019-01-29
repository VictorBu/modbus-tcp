using System;
using System.Collections.Generic;
using System.Text;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using Karonda.ModbusTcp.Entity.Function.Response;

namespace Karonda.ModbusTcp.Server
{
    public class ModbusResponse : IModbusResponseService
    {
        public ModbusFunction ReadHoldingRegisters(ReadHoldingRegistersRequest request)
        {
            var registers = new ushort[request.Quantity];

            Random ran = new Random();
            for (int i=0;i<registers.Length;i++)
            {
                registers[i] = (ushort)ran.Next(ushort.MinValue, ushort.MaxValue);
            }

            var response = new ReadHoldingRegistersResponse(registers);

            return response;
        }
    }
}
