using System;

namespace ModbusTest
{
    class Program
    {

        static ModbusManager manager;
        static bool running = true;

        static void Main(string[] args)
        {
            manager = new ModbusManager();

            manager.Inititalize();

            while (running)
            {
                String input = GetUserInput();
                if (!input.Equals(""))
                {
                    ProcessCommand(input);
                }
            }


        }

        private static void ProcessCommand(string input)
        {
            String[] args = input.Split(' ');

            if (args.Length == 1)
            {
                if ("/exit".Equals(args[0]))
                {
                    manager.DisconnectDevices();
                    Console.WriteLine("Exiting.. Press any key to close.");
                    Console.ReadKey();
                    running = false;
                }
                if ("/lightshow".Equals(args[0]))
                {
                    Console.WriteLine("10 Second Light Show!");
                    manager.LightShow(10);
                    Console.WriteLine("Light Show is over :(");
                }
            }
            else if (args.Length == 2)
            {
                if ("/lightshow".Equals(args[0]))
                {
                    int seconds = Int32.Parse(args[1]);
                    Console.WriteLine("Light Show!");
                    manager.LightShow(seconds);
                    Console.WriteLine("Light Show is over :(");
                }
            }
            else if (args.Length == 3)
            {
                if ("/set".Equals(args[0]))
                {
                    String result;
                    int output = Int32.Parse(args[1]);
                    if ("on".Equals(args[2]))
                    {
                        manager.SetDigitalOut(output, 1, out result);
                        Console.WriteLine(result);
                    }
                    else if ("off".Equals(args[2]))
                    {
                        manager.SetDigitalOut(output, 0, out result);
                        Console.WriteLine(result);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Arguments...");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Arguments...");
            }

        }

        static String GetUserInput()
        {
            return Console.ReadLine();
        }
    }
}
