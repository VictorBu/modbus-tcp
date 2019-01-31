using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using Karonda.ModbusTcp.Entity.Function.Response;

namespace Karonda.ModbusTcp.Server
{
    public class ModbusResponse : ModbusResponseService
    {
        public override ModbusFunction ReadCoils(ReadCoilsRequest request)
        {
            var coilArray = ReadCoilsOrInputs(request.Quantity);
            var response = new ReadCoilsResponse(coilArray);

            return response;
        }

        public override ModbusFunction ReadDiscreteInputs(ReadDiscreteInputsRequest request)
        {
            var inputArray = ReadCoilsOrInputs(request.Quantity);
            var response = new ReadDiscreteInputsResponse(inputArray);

            return response;
        }

        public override ModbusFunction ReadHoldingRegisters(ReadHoldingRegistersRequest request)
        {
            var registers = ReadRegisters(request.Quantity);
            var response = new ReadHoldingRegistersResponse(registers);

            return response;
        }

        public override ModbusFunction ReadInputRegisters(ReadInputRegistersRequest request)
        {
            var registers = ReadRegisters(request.Quantity);
            var response = new ReadInputRegistersResponse(registers);

            return response;
        }

        public override ModbusFunction WriteSingleCoil(WriteSingleCoilRequest request)
        {
            var response = new WriteSingleCoilResponse(request.StartingAddress, request.State);
            return response;
        }

        public override ModbusFunction WriteSingleRegister(WriteSingleRegisterRequest request)
        {
            var response = new WriteSingleRegisterResponse(request.StartingAddress, request.Value);
            return response;
        }

        private BitArray ReadCoilsOrInputs(ushort quantity)
        {
            var length = quantity + (8 - quantity % 8) % 8;
            var coils = new bool[length];

            Random ran = new Random();
            // from low to high
            for (int i = 0; i < coils.Length; i++)
            {
                if (i < quantity)
                {
                    coils[i] = ran.Next() % 2 == 0;
                    //coils[i] = true;
                }
                else
                {
                    coils[i] = false;
                }
            }

            var arr = new BitArray(coils);
            return arr;
        }

        private ushort[] ReadRegisters(ushort quantity)
        {
            var registers = new ushort[quantity];

            Random ran = new Random();
            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = (ushort)ran.Next(ushort.MinValue, ushort.MaxValue);
            }

            return registers;
        }
    }
}
