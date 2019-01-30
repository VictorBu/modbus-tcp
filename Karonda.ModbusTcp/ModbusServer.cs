using DotNetty.Codecs;
using DotNetty.Common.Internal.Logging;
using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Karonda.ModbusTcp.Handler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Karonda.ModbusTcp
{
    public class ModbusServer
    {
        private ModbusResponseService responseService;
        private ServerState serverState;
        public int Port { get; }
        public IChannel Channel { get; private set; }
        private IEventLoopGroup bossGroup;
        private IEventLoopGroup workerGroup;
        public ModbusServer(ModbusResponseService responseService, int port = 502)
        {
            this.responseService = responseService;
            Port = port;
            serverState = ServerState.NotStarted;
        }

        public async Task Start()
        {
            bossGroup = new MultithreadEventLoopGroup(1);
            workerGroup = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new ServerBootstrap();
                bootstrap.Group(bossGroup, workerGroup);

                bootstrap
                    .Channel<TcpServerSocketChannel>()
                    .Option(ChannelOption.SoBacklog, 100)
                    //.Handler(new LoggingHandler("SRV-LSTN"))
                    .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        //pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                        //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("encoder", new ModbusEncoder());
                        pipeline.AddLast("decoder", new ModbusDecoder(true));

                        pipeline.AddLast("request", new ModbusRequestHandler(responseService));
                    }));

                serverState = ServerState.Starting;

                Channel = await bootstrap.BindAsync(Port);

                serverState = ServerState.Started;
            }
            finally
            {
                
            }
        }

        public async Task Stop()
        {
            if (ServerState.Starting == serverState)
            {
                try
                {
                    await Channel.CloseAsync();
                }
                finally
                {
                    await Task.WhenAll(
                        bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                        workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));

                    serverState = ServerState.NotStarted;
                }
            }
        }
    }

    public enum ServerState
    {
        NotStarted = 0,
        Started = 1,
        Starting = 2,
    }
}
