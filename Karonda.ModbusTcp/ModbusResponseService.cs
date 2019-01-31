using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp
{
    public abstract class ModbusResponseService
    {
        public ModbusFunction Execute(ModbusFunction function)
        {
            if (function is ReadCoilsRequest)
            {
                var request = (ReadCoilsRequest)function;
                return ReadCoils(request);
            }
            else if (function is ReadDiscreteInputsRequest)
            {
                var request = (ReadDiscreteInputsRequest)function;
                return ReadDiscreteInputs(request);
            }
            else if (function is ReadHoldingRegistersRequest)
            {
                var request = (ReadHoldingRegistersRequest)function;
                return ReadHoldingRegisters(request);

            }
            else if (function is ReadInputRegistersRequest)
            {
                var request = (ReadInputRegistersRequest)function;
                return ReadInputRegisters(request);
            }
            else if(function is WriteSingleCoilRequest)
            {
                var request = (WriteSingleCoilRequest)function;
                return WriteSingleCoil(request);
            }
            else if(function is WriteSingleRegisterRequest)
            {
                var request = (WriteSingleRegisterRequest)function;
                return WriteSingleRegister(request);
            }
            else if(function is WriteMultipleCoilsRequest)
            {
                var request = (WriteMultipleCoilsRequest)function;
                return WriteMultipleCoils(request);
            }
            else if(function is WriteMultipleRegistersRequest)
            {
                var request = (WriteMultipleRegistersRequest)function;
                return WriteMultipleRegisters(request);
            }

            throw new Exception("Function Not Support");
        }
        public abstract ModbusFunction ReadCoils(ReadCoilsRequest request);

        public abstract ModbusFunction ReadDiscreteInputs(ReadDiscreteInputsRequest request);

        public abstract ModbusFunction ReadHoldingRegisters(ReadHoldingRegistersRequest request);

        public abstract ModbusFunction ReadInputRegisters(ReadInputRegistersRequest request);

        public abstract ModbusFunction WriteSingleCoil(WriteSingleCoilRequest request);

        public abstract ModbusFunction WriteSingleRegister(WriteSingleRegisterRequest request);

        public abstract ModbusFunction WriteMultipleCoils(WriteMultipleCoilsRequest request);

        public abstract ModbusFunction WriteMultipleRegisters(WriteMultipleRegistersRequest request);
    }
}
