using System;
using System.IO;

namespace VM
{
    class TestCases
    {
        public static void Testprogram(string[] args)
        {
            //load test.osx
            //run and print out result
            int len = args.Length;
            if (len == 1)
            {
                string temp = args[0];
                args = new string[2];
                args[0] = temp;
                args[1] = "Test.osx";
            }
            Console.WriteLine("Starting Load");
            Load(args);
            Console.WriteLine("\nFinished Load");
            args[0] = "Run";
            Console.WriteLine("Starting Run");
            Run(args);
            Console.WriteLine("Finished Run");
            Console.Write("Memory at end of run:");
            Instructions.Coredump();

        }
        private static void Load(string[] args)
        {
            try
            {
                string Filename = args[1];
                using (BinaryReader reader = new BinaryReader(File.Open(Filename, FileMode.Open)))
                {
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
                    int Testint = 0;
                    //Load program into memory
                    Console.Write("Loaded Program: ");
                    while (size > 0)
                    {
                        ///////////This is the testing part////////////
                        if (Testint == PC) { Console.Write("PC"); }
                        Testint++;
                        Console.Write("{0} ", Newprogram[instruction].ToString());
                        //////////////////////////////////////////////
                        Memory.SetMem(instruction, Page, Newprogram[instruction++]);
                        size--;
                    }
                    //set runtime stack registers
                    int Top = 0;
                    Memory.SetReg(6, Top); //Top
                    Memory.SetReg(7, Top); // next
                    Memory.SetReg(8, (PC - 1)); //Bottom
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Unable to open file due to {0}", ex);
            }
        }
        private static void Run(string[] args)
        {
            int page = 0;
            while (Memory.GetReg(11) != Memory.GetReg(10))
            {
                int optcode = Memory.GetMem(Memory.PC(), page);
                Console.WriteLine("Runing Opt Code {0}", optcode);
                OptCode.ConvertOpCode(optcode, page);
                int i = 0;
                Console.WriteLine("Registers");
                //PC 11 Point to next instruction to execute. Never directly set this register using assembly code. 
                //MD 12 Flage for what mode 0 = Kernal, 1 = User
                while (i < 6)
                {
                    Console.Write("R{0}:{1} ", i, Memory.GetReg(i++));
                }
                Console.Write("SP:{1} ", i, Memory.GetReg(i++));
                Console.Write("FP:{1} ", i, Memory.GetReg(i++));
                Console.Write("SL:{1} ", i, Memory.GetReg(i++));
                Console.Write("Z:{1} ", i, Memory.GetReg(i++));
                Console.Write("SB:{1} ", i, Memory.GetReg(i++));
                Console.Write("PC:{1} ", i, Memory.GetReg(i++));
                Console.Write("MD:{1} \n\n", i, Memory.GetReg(i++));
            }
        }
    }
}
