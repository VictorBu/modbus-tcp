using DotNetty.Transport.Channels;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Karonda.ModbusTcp.Handler
{
    public class ModbusResponseHandler : SimpleChannelInboundHandler<ModbusFrame>
    {
        private readonly int timeoutMilliseconds = 2000;
        private Dictionary<ushort, ModbusFrame> responses = new Dictionary<ushort, ModbusFrame>();
        protected override void ChannelRead0(IChannelHandlerContext ctx, ModbusFrame msg)
        {
            responses.Add(msg.Header.TransactionIdentifier, msg);
        }

        public ModbusFrame GetResponse(ushort transactionIdentifier)
        {
            ModbusFrame frame = null;
            var timeoutDateTime = DateTime.Now.AddMilliseconds(timeoutMilliseconds);
            do
            {
                Thread.Sleep(1);
                if (responses.ContainsKey(transactionIdentifier))
                {
                    frame = responses[transactionIdentifier];
                    responses.Remove(transactionIdentifier);
                }
            }
            while (frame == null && DateTime.Now < timeoutDateTime);

            if(frame == null)
            {
                throw new Exception("No Response");
            }
            else if(frame.Function is ExceptionFunction)
            {
                throw new Exception(frame.Function.ToString());
            }

            return frame;
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            context.CloseAsync();
        }
    }
}
