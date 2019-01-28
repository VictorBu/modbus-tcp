using DotNetty.Transport.Channels;
using Karonda.ModbusTcp.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Karonda.ModbusTcp.Handler
{
    public class ModbusEncoder : ChannelHandlerAdapter
    {
        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (message is ModbusFrame)
            {
                var frame = (ModbusFrame)message;
                return context.WriteAndFlushAsync(frame.Encode());
            }

            return context.WriteAsync(message);
        }
    }
}
