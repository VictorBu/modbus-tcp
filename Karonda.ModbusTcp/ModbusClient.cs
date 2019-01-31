using DotNetty.Handlers.Logging;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Request;
using Karonda.ModbusTcp.Entity.Function.Response;
using Karonda.ModbusTcp.Handler;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Karonda.ModbusTcp
{
    public class ModbusClient
    {
        public string Ip { get; }
        public int Port { get; }
        public short UnitIdentifier { get; }
        public IChannel Channel { get; private set; }

        private MultithreadEventLoopGroup group;
        private ConnectionState connectionState;
        private ushort transactionIdentifier;
        private readonly string handlerName = "response";

        public ModbusClient(short unitIdentifier, string ip, int port = 502)
        {
            Ip = ip;
            Port = port;
            UnitIdentifier = unitIdentifier;

            connectionState = ConnectionState.NotConnected;
        }

        public async Task Connect()
        {
            group = new MultithreadEventLoopGroup();

            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        //pipeline.AddLast(new LoggingHandler());
                        //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("encoder", new ModbusEncoder());
                        pipeline.AddLast("decoder", new ModbusDecoder(false));

                        pipeline.AddLast(handlerName, new ModbusResponseHandler());
                    }));

                connectionState = ConnectionState.Pending;

                Channel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse(Ip), Port));

                connectionState = ConnectionState.Connected;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task Close()
        {
            if (ConnectionState.Connected == connectionState)
            {
                try
                {
                    await Channel.CloseAsync();
                }
                finally
                {
                    await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));

                    connectionState = ConnectionState.NotConnected;
                }
            }
        }

        public ushort CallModbusFunction(ModbusFunction function)
        {
            if (ConnectionState.Connected != connectionState || Channel == null)
            {
                throw new Exception("Not connected!");
            }

            SetTransactionIdentifier();

            ModbusHeader header = new ModbusHeader(transactionIdentifier, UnitIdentifier);
            ModbusFrame frame = new ModbusFrame(header, function);
            Channel.WriteAndFlushAsync(frame);

            return transactionIdentifier;
        }

        public T CallModbusFunctionSync<T>(ModbusFunction function) where T : ModbusFunction
        {
            var transactionIdentifier = CallModbusFunction(function);

            var handler = (ModbusResponseHandler)Channel.Pipeline.Get(handlerName);
            if (handler == null)
            {
                throw new Exception("Not connected!");
            }

            return (T)handler.GetResponse(transactionIdentifier).Function;
        }

        private void SetTransactionIdentifier()
        {
            if (transactionIdentifier < ushort.MaxValue)
            {
                transactionIdentifier++;
            }
            else
            {
                transactionIdentifier = 1;
            }
        }

        public ushort ReadCoilsAsync(ushort startingAddress, ushort quantity)
        {
            var function = new ReadCoilsRequest(startingAddress, quantity);
            return CallModbusFunction(function);
        }

        public ushort ReadDiscreteInputsAsync(ushort startingAddress, ushort quantity)
        {
            var function = new ReadDiscreteInputsRequest(startingAddress, quantity);
            return CallModbusFunction(function);
        }

        public ushort ReadHoldingRegistersAsync(ushort startingAddress, ushort quantity)
        {
            var function = new ReadHoldingRegistersRequest(startingAddress, quantity);
            return CallModbusFunction(function);
        }

        public ushort ReadInputRegistersAsync(ushort startingAddress, ushort quantity)
        {
            var function = new ReadInputRegistersRequest(startingAddress, quantity);
            return CallModbusFunction(function);
        }

        public ushort WriteSingleCoilAsync(ushort startingAddress, bool state)
        {
            var function = new WriteSingleCoilRequest(startingAddress, state);
            return CallModbusFunction(function);
        }

        public ushort WriteSingleRegisterAsync(ushort startingAddress, ushort value)
        {
            var function = new WriteSingleRegisterRequest(startingAddress, value);
            return CallModbusFunction(function);
        }

        public ushort WriteMultipleCoilsAsync(ushort startingAddress, bool[] states)
        {
            var function = new WriteMultipleCoilsRequest(startingAddress, states);
            return CallModbusFunction(function);
        }

        public ushort WriteMultipleRegistersAsync(ushort startingAddress, ushort[] registers)
        {
            var function = new WriteMultipleRegistersRequest(startingAddress, registers);
            return CallModbusFunction(function);
        }



        public ReadCoilsResponse ReadCoils(ushort startingAddress, ushort quantity)
        {
            var function = new ReadCoilsRequest(startingAddress, quantity);
            return CallModbusFunctionSync<ReadCoilsResponse>(function);
        }

        public ReadDiscreteInputsResponse ReadDiscreteInputs(ushort startingAddress, ushort quantity)
        {
            var function = new ReadDiscreteInputsRequest(startingAddress, quantity);
            return CallModbusFunctionSync<ReadDiscreteInputsResponse>(function);
        }

        public ReadHoldingRegistersResponse ReadHoldingRegisters(ushort startingAddress, ushort quantity)
        {
            var function = new ReadHoldingRegistersRequest(startingAddress, quantity);
            return CallModbusFunctionSync<ReadHoldingRegistersResponse>(function);
        }

        public ReadInputRegistersResponse ReadInputRegisters(ushort startingAddress, ushort quantity)
        {
            var function = new ReadInputRegistersRequest(startingAddress, quantity);
            return CallModbusFunctionSync<ReadInputRegistersResponse>(function);
        }

        public WriteSingleCoilResponse WriteSingleCoil(ushort startingAddress, bool state)
        {
            var function = new WriteSingleCoilRequest(startingAddress, state);
            return CallModbusFunctionSync<WriteSingleCoilResponse>(function);
        }

        public WriteSingleRegisterResponse WriteSingleRegister(ushort startingAddress, ushort value)
        {
            var function = new WriteSingleRegisterRequest(startingAddress, value);
            return CallModbusFunctionSync<WriteSingleRegisterResponse>(function);
        }

        public WriteMultipleCoilsResponse WriteMultipleCoils(ushort startingAddress, bool[] states)
        {
            var function = new WriteMultipleCoilsRequest(startingAddress, states);
            return CallModbusFunctionSync<WriteMultipleCoilsResponse>(function);
        }

        public WriteMultipleRegistersResponse WriteMultipleRegisters(ushort startingAddress, ushort[] registers)
        {
            var function = new WriteMultipleRegistersRequest(startingAddress, registers);
            return CallModbusFunctionSync<WriteMultipleRegistersResponse>(function);
        }

    }



    public enum ConnectionState
    {
        NotConnected = 0,
        Connected = 1,
        Pending = 2,
    }
}
