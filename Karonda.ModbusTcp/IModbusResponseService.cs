using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp
{
    public interface IModbusResponseService
    {
        ModbusFunction ReadHoldingRegisters(ReadHoldingRegistersRequest request);

        ModbusFunction ReadDiscreteInputs(ReadDiscreteInputsRequest request);

        ModbusFunction ReadCoils(ReadCoilsRequest request);
    }
}
