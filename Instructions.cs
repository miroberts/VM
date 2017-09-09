using System;
using System.IO;

namespace VM
{
	public class Instructions
	{
		public static void Shutdown()
        {
            //
        }
		public static void Load(string[] args)
		{
            try
            {
                string Filename = args[1];
                using (BinaryReader reader = new BinaryReader(File.Open(Filename, FileMode.Open)))
                {
                    Console.WriteLine("Opening Program.");
                    int instruction = 0;
                    //Read file
                    int SB = reader.ReadInt32();
                    int PC = reader.ReadInt32();
                    int Page = reader.ReadInt32();
                    byte[] Newprogram = reader.ReadBytes(SB);
                    int size = Newprogram.Length;

                    //Load Registers
                    Memory.SetReg(10, size);
                    Memory.SetReg(11, PC);
                    //Load program into memory
                    while (size > 0)
                    {
                        Memory.SetMem(instruction, Page, Newprogram[instruction++]);
                        size--;
                    }
                    //set runtime stack registers
                    int Top = 0;
                    Memory.SetReg(6, Top); //Top
                    Memory.SetReg(7, Top); // next
                    Memory.SetReg(8, (PC - 1)); //Bottom

                    Console.WriteLine("Loaded Program.");    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: Unable to open file due to {0}", ex);
            }
		}
		public static void Run(string[] args)
		{
            int page = 0;
            while(Memory.GetReg(11) != Memory.GetReg(10))
            {
                OptCode.ConvertOpCode(Memory.GetMem(Memory.PC(), page), page);
            }
		}
        public static void Coredump()
        {
            int size = 0;
            int limit = Memory.GetReg(10);
            int page = 0;
            byte[] data = new byte[limit];

            while (size < limit)
            {
                data[size] = Memory.GetMem(size++, page);
                //Console.WriteLine("{0}  {1}  {2}  {3}  {4}  {5}", Memory.GetMem(size++, page).ToString("X2"), Memory.GetMem(size++, page).ToString("X2"), Memory.GetMem(size++, page).ToString("X2"), Memory.GetMem(size++, page).ToString("X2"), Memory.GetMem(size++, page).ToString("X2"), Memory.GetMem(size++, page).ToString("X2"));
            }
            Console.WriteLine("{0}", BitConverter.ToString(data));
        }
        public static void Help()
        {
            Console.WriteLine("List of Commands:");
            Console.WriteLine("\tLoad <nameofprogram>.osx");
            Console.WriteLine("\tRun");
            Console.WriteLine("\tCoredump");
            Console.WriteLine("\tErrordump");
            Console.WriteLine("\tTest <nameofprogram>.osx -if no file name is given it defaults to Test.osx");
            Console.WriteLine("\tQuit");
        }
        public static void Errordump()
        {
            Console.WriteLine("All of the errors print to the Console as they happen for now, Here are examples.");
            Console.WriteLine("Error: Invalid command");
            Console.WriteLine("Error: Unable to open file");
            Console.WriteLine("Error: No op code found at memory location");
            Console.WriteLine("Error: No interrupt found");
        }
    }
}
