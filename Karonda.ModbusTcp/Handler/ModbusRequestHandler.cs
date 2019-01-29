using DotNetty.Transport.Channels;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karonda.ModbusTcp.Handler
{
    public class ModbusRequestHandler : SimpleChannelInboundHandler<ModbusFrame>
    {
        private IModbusResponseService responseService;
        public ModbusRequestHandler(IModbusResponseService responseService)
        {
            this.responseService = responseService;
        }

        protected override void ChannelRead0(IChannelHandlerContext ctx, ModbusFrame msg)
        {
            var function = msg.Function;
            ModbusFunction response = null;

            if(function is ReadCoilsRequest)
            {
                var request = (ReadCoilsRequest)function;
                response = responseService.ReadCoils(request);
            }
            else if(function is ReadHoldingRegistersRequest)
            {
                var request = (ReadHoldingRegistersRequest)function;
                response = responseService.ReadHoldingRegisters(request);

            }
            else
            {
                throw new Exception("Function Not Support");
            }

            var header = msg.Header;
            var frame = new ModbusFrame(header, response);

            ctx.WriteAndFlushAsync(frame);
        }
    }
}
