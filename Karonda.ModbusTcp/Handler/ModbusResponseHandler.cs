using DotNetty.Transport.Channels;
using Karonda.ModbusTcp.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Karonda.ModbusTcp.Handler
{
    public class ModbusResponseHandler : SimpleChannelInboundHandler<ModbusFrame>
    {
        private Dictionary<ushort, ModbusFrame> responses = new Dictionary<ushort, ModbusFrame>();
        protected override void ChannelRead0(IChannelHandlerContext ctx, ModbusFrame msg)
        {
            responses.Add(msg.Header.TransactionIdentifier, msg);
        }

        public ModbusFrame GetResponse(ushort transactionIdentifier)
        {
            ModbusFrame frame = null;
            do
            {
                Thread.Sleep(1);
                if (responses.ContainsKey(transactionIdentifier))
                {
                    frame = responses[transactionIdentifier];
                    responses.Remove(transactionIdentifier);
                }
            }
            while (frame == null);

            return frame;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
        }
    }
}
