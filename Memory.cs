using System;

namespace VM
{
    public class Memory
    {
        //Memory
		private static byte[,] MemoryBlock = new byte[100,100];


        public static void SetMem(int location, int page, byte value)
        {
            if (location < Registers[10] && location >= 0 && page < 100 && page >= 0)
            {
                MemoryBlock[page, location] = value;
            }
            else
            {
                Console.WriteLine("Error: Memory request out of bounds");
            }
        }

        public static byte GetMem(int location, int page)
        {
            if (location < 100 && location >=0  && Registers[10] >= 0 && page < 100 && page >= 0)
            {
                return MemoryBlock[page, location];
            }
            else
            {
                Console.WriteLine("Error: Memory request out of bounds");
                return 0;
            }
        }



        // Registers
        //R0, R1, R2, R3, R4, R5, General Purpose
        //SP 6 Point at top of run-time stack, used with SL register to test for Stack-Overflow and Out-Of-Memory.
        //FP 7 Point at current frame on run-time stack
        //SL 8 Limit the size of the run-time stack, set this register in your VM to the byteSize value stored as the first value in the osX byte file. Can be used to test for Stack-Overflow and Out-Of-Memory
        //Z  9 Set when CMP instruction is run, used by conditional branch instructions
        //SB 10 Limit the size of memory in your VM, set this register in your VM to the maximum size of memory. Can be used to test for Stack-Underflow.
        //PC 11 Point to next instruction to execute. Never directly set this register using assembly code. 
        //MD 12 Flage for what mode 0 = Kernal, 1 = User
        private static int[] Registers = new int [13];

        // Increease PC by 1
        public static void increasePC()
        {
            int temp = Registers[11];
            //if (temp < Registers[10])
            try
            {
                Registers[11] = temp + 1;
            }
            //else
            catch(Exception ex)
            {
                Console.WriteLine("Error: PC reached the end");
            }
        }

        // Return the PC and call increasePC
        public static int PC()
        {
            try
            //if (Registers[11] < Registers[10])
            {
                int pc = Registers[11];
                increasePC();
                return pc;
            }
            //else
            catch (Exception ex)
            {
                return -1;
            }
        }

        // Increease FP by 1
        private static void increaseFP()
        {
            int temp = Registers[7];
            if (temp <= Registers[8] && temp >= Registers[6])
            {
                Registers[7] = temp + 1;
            }
            else
            {
                Console.WriteLine("Error: FP reached the end");
            }
        }

        // Return the PC and call increasePC
        public static int FP()
        {
            int temp = Registers[7];
            if (temp <= Registers[8] && temp >= Registers[6])
            {
                int fp = Registers[7];
                increaseFP();
                return fp;
            }
            else
            {
                return -1;
            }
        }

        public static void SetReg(int reg, int value)
        {
            if (reg < 13 && reg >= 0)
            {
                Registers[reg] = value;
            }
            else
            {
                Console.WriteLine("Error: Register request out of bounds");
            }
        }

        public static int GetReg(int reg)
        {
            if (reg < 13 && reg >= 0)
            {
                return Registers[reg];
            }
            else
            {
                Console.WriteLine("Error: Register request out of bounds");
                return -1;
            }
        }


    }// end class Memory()
}