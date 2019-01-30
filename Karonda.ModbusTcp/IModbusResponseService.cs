using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp
{
    public interface IModbusResponseService
    {
        ModbusFunction ReadCoils(ReadCoilsRequest request);

        ModbusFunction ReadDiscreteInputs(ReadDiscreteInputsRequest request);
        
        ModbusFunction ReadHoldingRegisters(ReadHoldingRegistersRequest request);

        ModbusFunction ReadInputRegisters(ReadInputRegistersRequest request);
    }
}
