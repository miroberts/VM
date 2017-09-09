using System;

namespace VM
{
	public class VM
	{
		public static void Main()
		{
            Console.WriteLine("For help type \"Help\"");
            while (true) // while system is on
			{
                Console.Write("VM>");
				var line = Console.ReadLine();
				var args = line.Split();

				switch(args[0].ToLower())
            	{
                    case "":
                        break;
                    case "load":
						Instructions.Load(args);
						break;
					case "run":
						Instructions.Run(args);
						break;
					case "quit":
						Instructions.Shutdown();
						Environment.Exit(0);
						break;
                    case "coredump":
                        Instructions.Coredump();
                        break;
                    case "errordump":
                        Instructions.Errordump();
                        break;
                    case "test":
                        TestCases.Testprogram(args);
                        break;
                    case "help":
                        Instructions.Help();
                        break;
                    default:
                    	Console.WriteLine("Error: Invalid command");
						break;
				}
			}// end while
		}// end Main
	} //end class VM
}