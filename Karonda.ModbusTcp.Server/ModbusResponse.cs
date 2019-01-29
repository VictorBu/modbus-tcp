using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using Karonda.ModbusTcp.Entity.Function.Response;

namespace Karonda.ModbusTcp.Server
{
    public class ModbusResponse : IModbusResponseService
    {
        public ModbusFunction ReadCoils(ReadCoilsRequest request)
        {
            var length = request.Quantity + (8 - request.Quantity % 8) % 8;
            var coils = new bool[length];

            Random ran = new Random();
            // from low to high
            for (int i = 0; i<coils.Length; i++)
            {
                if(i < request.Quantity)
                {
                    coils[i] = ran.Next() % 2 == 0;
                }
                else
                {
                    coils[i] = false;
                }
            }

            var coilArray = new BitArray(coils);
            var response = new ReadCoilsResponse(coilArray);

            return response;
        }

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
