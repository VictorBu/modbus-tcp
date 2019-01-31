using DotNetty.Common.Internal.Logging;
using Karonda.ModbusTcp.Entity;
using Karonda.ModbusTcp.Entity.Function.Response;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Threading.Tasks;

namespace Karonda.ModbusTcp.Client
{
    class Program
    {
        static async Task RunClientAsync()
        {
            //InternalLoggerFactory.DefaultFactory.AddProvider(new ConsoleLoggerProvider((s, level) => true, false));

            ModbusClient client = new ModbusClient(0x01, "127.0.0.1");

            try
            {
                await client.Connect();

                while(true)
                {
                    Console.WriteLine(@"
<------------------------------------------------------->
1: Read Coils; 2: Read Discrete Inputs; 
3: Read Holding Registers; 4: Read Input Registers; 
5: Write Single Coil; 6: Write Single Register; 
15: Write Multiple Coils; 16: Write Multiple Registers;
<------------------------------------------------------->");
                    var line = Console.ReadLine();
                    if (string.IsNullOrEmpty(line)) break;

                    Console.WriteLine("<------------------------------------------------------->");
                    var command = Convert.ToInt32(line);

                    ModbusFunction response = null;
                    ushort startingAddress = 0x0000;
                    ushort quantity = 0x000A;
                    var state = true;
                    ushort value = 0x0001;

                    switch (command)
                    {
                        case 1:                            
                            response = client.ReadCoils(startingAddress, quantity);
                            var coils = (response as ReadCoilsResponse).Coils;
                            for(int i =0;i< quantity; i++)
                            {
                                Console.WriteLine(coils[i]);
                            }
                            break;
                        case 2:
                            response = client.ReadDiscreteInputs(startingAddress, quantity);
                            var inputs = (response as ReadDiscreteInputsResponse).Inputs;
                            for (int i = 0; i < quantity; i++)
                            {
                                Console.WriteLine(inputs[i]);
                            }
                            break;
                        case 3:
                            response = client.ReadHoldingRegisters(startingAddress, quantity);
                            foreach(var register in (response as ReadHoldingRegistersResponse).Registers)
                            {
                                Console.WriteLine(register);
                            }
                            break;
                        case 4:
                            response = client.ReadInputRegisters(startingAddress, quantity);
                            foreach (var register in (response as ReadInputRegistersResponse).Registers)
                            {
                                Console.WriteLine(register);
                            }
                            break;
                        case 5:
                            response = client.WriteSingleCoil(startingAddress, state);
                            Console.WriteLine((response as WriteSingleCoilResponse).State == state ? "Successed" : "Failed");
                            break;
                        case 6:
                            response = client.WriteSingleRegister(startingAddress, value);
                            Console.WriteLine((response as WriteSingleRegisterResponse).Value == value ? "Successed" : "Failed");
                            break;
                        case 15:
                            var states = new bool[] { true, true, true, true, true, true, true, true, true, true };
                            response = client.WriteMultipleCoils(startingAddress, states);
                            Console.WriteLine((response as WriteMultipleCoilsResponse).Quantity == states.Length);
                            break;
                        case 16:
                            var registers = new ushort[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
                            response = client.WriteMultipleRegisters(startingAddress, registers);
                            Console.WriteLine((response as WriteMultipleRegistersResponse).Quantity == registers.Length);
                            break;
                    }

                }

                await client.Close();

                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        static void Main() => RunClientAsync().Wait();
    }
}
