using System;
using System.Collections.Generic;
using System.Text;
using DotNetty.Buffers;

namespace Karonda.ModbusTcp.Entity.Function
{
/*
* Modbus Exception Codes
*
* 01 ILLEGAL FUNCTION
*
* The function code received in the query is not an allowable action for
* the server (or slave). This may be because the function code is only
* applicable to newer devices, and was not implemented in the unit
* selected. It could also indicate that the server (or slave) is in the
* wrong state to process a request of this type, for example because it is
* unconfigured and is being asked to return register values.
*
* 02 ILLEGAL DATA ADDRESS
*
* The data address received in the query is not an allowable address for
* the server (or slave). More specifically, the combination of reference
* number and transfer length is invalid. For a controller with 100
* registers, the PDU addresses the first register as 0, and the last one as
* 99. If a request is submitted with a starting register address of 96 and
* a quantity of registers of 4, then this request will successfully operate
* (address-wise at least) on registers 96, 97, 98, 99. If a request is
* submitted with a starting register address of 96 and a quantity of
* registers of 5, then this request will fail with Exception Code 0x02
* “Illegal Data Address” since it attempts to operate on registers 96, 97,
* 98, 99 and 100, and there is no register with address 100.
*
* 03 ILLEGAL DATA VALUE
*
* A value contained in the query data field is not an allowable value for
* server (or slave). This indicates a fault in the structure of the
* remainder of a complex request, such as that the implied length is
* incorrect. It specifically does NOT mean that a data item submitted for
* storage in a register has a value outside the expectation of the
* application program, since the MODBUS protocol is unaware of the
* significance of any particular value of any particular register.
*
* 04 SLAVE DEVICE FAILURE
*
* An unrecoverable error occurred while the server (or slave) was
* attempting to perform the requested action.
*
* 05 ACKNOWLEDGE
*
* Specialized use in conjunction with programming commands. The server (or
* slave) has accepted the request and is processing it, but a long duration
* of time will be required to do so. This response is returned to prevent a
* timeout error from occurring in the client (or master). The client (or
* master) can next issue a Poll Program Complete message to determine if
* processing is completed.
*
* 06 SLAVE DEVICE BUSY
*
* Specialized use in conjunction with programming commands. The server (or
* slave) is engaged in processing a long–duration program command. The
* client (or master) should retransmit the message later when the server
* (or slave) is free.
*
* 08 MEMORY PARITY ERROR
*
* Specialized use in conjunction with function codes 20 and 21 and
* reference type 6, to indicate that the extended file area failed to pass
* a consistency check. The server (or slave) attempted to read record file,
* but detected a parity error in the memory. The client (or master) can
* retry the request, but service may be required on the server (or slave)
* device.
*
* 0A GATEWAY PATH UNAVAILABLE
*
* Specialized use in conjunction with gateways, indicates that the gateway
* was unable to allocate an internal communication path from the input port
* to the output port for processing the request. Usually means that the
* gateway is misconfigured or overloaded.
*
* 0B GATEWAY TARGET DEVICE FAILED TO RESPOND
*
* Specialized use in conjunction with gateways, indicates that no response
* was obtained from the target device. Usually means that the device is not
* present on the network.
*/
    public class ExceptionFunction : ModbusFunction
    {
        public short ExceptionCode { get; private set; }
        public string ExceptionMessage
        {
            get
            {
                if (exceptions.ContainsKey(ExceptionCode))
                {
                    return exceptions[ExceptionCode];
                }
                return "Undefined Error";
            }
        }
        private static readonly Dictionary<short, string> exceptions = new Dictionary<short, string>
        {
            {0x01, "Illegal Function"},
            {0x02, "Illegal Data Address"},
            {0x03, "Illegal Data Value"},
            {0x04, "Slave Device Failure"},
            {0x05, "Acknowledge"},
            {0x06, "Slave Device Busy"},
            {0x08, "Memory Parity Error"},
            {0x0A, "Gateway Path Unavailable"},
            {0x0B, "Gateway Target Device Failed To Respond"},

        };
        public ExceptionFunction(short functionCode)
            : base(functionCode)
        {
        }
        public ExceptionFunction(short functionCode, short exceptionCode)
            : base(functionCode)
        {
            ExceptionCode = exceptionCode;
        }
        public override int CalculateLength()
        {
            return 1;
        }

        public override void Decode(IByteBuffer buffer)
        {
            ExceptionCode = buffer.ReadByte();
        }

        public override IByteBuffer Encode()
        {
            IByteBuffer buffer = Unpooled.Buffer();
            buffer.WriteByte(FunctionCode);

            buffer.WriteByte(ExceptionCode);

            return buffer;
        }

        public override string ToString()
        {
            return $"Exception Code: {ExceptionCode}, Message: {ExceptionMessage}";
        }
    }
}
