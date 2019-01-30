using DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;

namespace Karonda.ModbusTcp.Server
{
    class Program
    {
        static async Task RunServerAsync()
        {
            //InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            ModbusResponse response = new ModbusResponse();
            ModbusServer server = new ModbusServer(response);

            await server.Start();

            Console.WriteLine("Server Started");
            Console.ReadLine();

            await server.Stop();
        }

        static void Main() => RunServerAsync().Wait();
    }
}
