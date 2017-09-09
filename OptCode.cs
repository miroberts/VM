using System;
using System.Collections.Generic;

namespace VM
{
    public class OptCode
    {
        // op code takes an int and retuns the corisponding opt code
        private static Dictionary<int, string> OpCodeMap = new Dictionary<int, string>
        {
            {0, "ADR"},
            { 1, "MOV"},
            { 2, "STR"},
            { 3, "STRB"},
            { 4, "LDR"},
            { 5, "LDRB"},
            { 6, "BX"},
            { 7, "B"},
            { 8, "BNE"},
            { 9, "BGT"},
            { 10, "BLT"},
            { 11, "BEQ"},
            { 12, "CMP"},
            { 13, "AND"},
            { 14, "ORR"},
            { 15, "EOR"},
            { 16, "ADD"},
            { 17, "SUB"},
            { 18, "MUL"},
            { 19, "DIV"},
            { 20, "SWI"},
            { 21, "BL"},
            { 22, "MVI"}
        };




        //Takes a string opt code name and calls its function
        public static void ConvertOpCode(int opcode, int page)
        {
            string Op;
            bool exists = OpCodeMap.TryGetValue(opcode, out Op);
            if (exists)
            {
                switch (Op)
                {
                    case "ADR":
                        ADR(page);
                        break;
                    case "MOV":
                        MOV(page);
                        break;
                    case "STR":
                        STR(page);
                        break;
                    case "STRB":
                        STRB(page);
                        break;
                    case "LDR":
                        LDR(page);
                        break;
                    case "LDRB":
                        LDRB(page);
                        break;
                    case "BX":
                        BX(page);
                        break;
                    case "B":
                        B(page);
                        break;
                    case "BNE":
                        BNE(page);
                        break;
                    case "BGT":
                        BGT(page);
                        break;
                    case "BLT":
                        BLT(page);
                        break;
                    case "BEQ":
                        BEQ(page);
                        break;
                    case "CMP":
                        CMP(page);
                        break;
                    case "AND":
                        AND(page);
                        break;
                    case "ORR":
                        ORR(page);
                        break;
                    case "EOR":
                        EOR(page);
                        break;
                    case "ADD":
                        ADD(page);
                        break;
                    case "SUB":
                        SUB(page);
                        break;
                    case "MUL":
                        MUL(page);
                        break;
                    case "DIV":
                        DIV(page);
                        break;
                    case "SWI":
                        SWI(page);
                        break;
                    case "BL":
                        BL(page);
                        break;
                    case "MVI":
                        MVI(page);
                        break;
                    default:
                        Console.WriteLine("Error: No op code found at memory location");
                        break;
                }// end Switch
            } // end if Opt Code exists
        }// end ConvertOpCode()

        // Opt Code Functions
        // Arithmetic
        //<reg1> <- < reg2> + < reg3>;	
        //1 byte op code | 1 byte register | 1 byte register | 1 byte register | 2 bytes unused
        private static void ADD(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            int reg3 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(reg1, (Memory.GetReg(reg2) + Memory.GetReg(reg3)));
            Memory.increasePC();
            Memory.increasePC();
        }
        //<reg1> <- < reg2> - < reg3>;	
        //1 byte op code | 1 byte register | 1 byte register | 1 byte register | 2 bytes unused
        private static void SUB(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            int reg3 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(reg1, (Memory.GetReg(reg2) - Memory.GetReg(reg3)));
            Memory.increasePC();
            Memory.increasePC();
        }

        //<reg1> <- < reg2> * < reg3>;	
        //1 byte op code | 1 byte register | 1 byte register | 1 byte register | 2 bytes unused
        private static void MUL(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            int reg3 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(reg1, (Memory.GetReg(reg2) * Memory.GetReg(reg3)));
            Memory.increasePC();
            Memory.increasePC();
        }

        //<reg1> <- < reg2> / < reg3>;	
        //1 byte op code | 1 byte register | 1 byte register | 1 byte register | 2 bytes unused
        private static void DIV(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            int reg3 = Memory.GetMem(Memory.PC(), page);
            try // reg3 can't hold 0
            {
                Memory.SetReg(reg1, (Memory.GetReg(reg2) / Memory.GetReg(reg3)));
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}", ex);
            }
            Memory.increasePC();
            Memory.increasePC();
        }


        // Move Data
        // <reg1> <- <reg2>; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void MOV(int page)
        {
            //<reg1> = R[Memoryblock[PC, page]]
            int reg1 = Memory.GetMem(Memory.PC(), page);
            // increase PC
            //load value into <reg1>
            Memory.SetReg(reg1, Memory.GetMem(Memory.PC(), page));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        //<reg1> <- <imm>
        //1 byte op code | 1 byte register | 4 byte address
        private static void MVI(int page)
        {
            //Get 4 bytes to get value and increase FP
            byte one = Memory.GetMem(Memory.FP(), page);
            byte two = Memory.GetMem(Memory.FP(), page);
            byte three = Memory.GetMem(Memory.FP(), page);
            byte four = Memory.GetMem(Memory.FP(), page);

            // increase PC
            int reg1 = (int)Memory.GetMem(Memory.PC(), page);

            //load value into <reg1>
            Memory.SetReg(reg1, AppendBytes(one, two, three, four));
        }

        // <reg1> <- address(<label>); 
        //1 byte op code | 1 byte register | 4 byte address
        private static void ADR(int page)
        {
            // get register
            int reg1 = Memory.GetMem(Memory.PC(), page);

            //Get 4 bytes to combind into lable and increase FP
            byte one = Memory.GetMem(Memory.PC(), page);
            byte two = Memory.GetMem(Memory.PC(), page);
            byte three = Memory.GetMem(Memory.PC(), page);
            byte four = Memory.GetMem(Memory.PC(), page);

            //load value into <reg1>
            Memory.SetReg(reg1, Memory.GetMem(AppendBytes(one, two, three, four), page));
        }

        // memory[<reg2>] <- <reg1>; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void STR(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetMem(Memory.GetReg(reg2), page, (byte)Memory.GetReg(reg1));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        //memory[<reg2>] <- byte(memory[<reg1>])
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused 
        private static void STRB(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetMem(Memory.GetReg(reg2), page, (byte)Memory.GetReg(reg1));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        // <reg1> <- memory[<reg2>] 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused 
        private static void LDR(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(reg1, Memory.GetReg(reg2));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        // <reg1> <- byte(memory[<reg2>]; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void LDRB(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(reg1, Memory.GetReg(reg2));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        //Branch
        // PC <- address(<label>); 
        //1 byte op code | 4 byte address | 1 byte unused
        private static void B(int page)
        {
            //Combind next 4 bytes and increase PC
            byte one = Memory.GetMem(Memory.FP(), page);
            byte two = Memory.GetMem(Memory.FP(), page);
            byte three = Memory.GetMem(Memory.FP(), page);
            byte four = Memory.GetMem(Memory.FP(), page);

            //load value into PC or 11
            Memory.SetReg(11, AppendBytes(one, two, three, four));
            Memory.increasePC();
        }

        //PC <- address(<label>)  R5 <- PC+6; 
        //1 byte op code | 4 byte address | 1 byte unused
        private static void BL(int page)
        {
            //Combind next 4 bytes and increase PC
            byte one = Memory.GetMem(Memory.FP(), page);
            byte two = Memory.GetMem(Memory.FP(), page);
            byte three = Memory.GetMem(Memory.FP(), page);
            byte four = Memory.GetMem(Memory.FP(), page);
            
            // Put PC of next instuction into R5
            Memory.SetReg(5, Memory.PC());
            //Jump to lable
            Memory.SetReg(11, AppendBytes(one, two, three, four));
            Memory.increasePC();
        }

        //PC <- <reg>; 
        //1 byte op code | 1 byte register | 4 bytes unused
        private static void BX(int page)
        {
            Memory.SetReg(11, Memory.GetMem(Memory.PC(), page));
        }

        //PC <- address(<label>) if Z != 0; 
        //1 byte op code | 4 byte address | 1 byte unused 
        private static void BNE(int page)
        {
            if(Memory.GetReg(9) != 0)
            {
                B(page);
            }
        }

        //PC <- address(<label>) if Z > 0; 
        //1 byte op code | 4 byte address | 1 byte unused
        private static void BGT(int page)
        {
            if (Memory.GetReg(9) > 0)
            {
                B(page);
            }
        }

        // PC <- address(<label>) if Z < 0; 
        //1 byte op code | 4 byte address | 1 byte unused 
        private static void BLT(int page)
        {
            if (Memory.GetReg(9) < 0)
            {
                B(page);
            }
        }

        // PC <- address(<label>) if Z == 0;  
        //1 byte op code | 4 byte address | 1 byte unused
        private static void BEQ(int page)
        {
            if (Memory.GetReg(9) == 0)
            {
                B(page);
            }
        }

        //Logical
        //Z <- <reg1> - <reg2>; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void CMP(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(9, (Memory.GetReg(reg1) - Memory.GetReg(reg2)));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        // Z <- <reg1> & < reg2> bitwise AND operation;
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void AND(int page)
        {
            byte reg1 = (byte)Memory.GetMem(Memory.PC(), page);
            byte reg2 = (byte)Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(9, (int)((byte)Memory.GetReg(reg1) & (byte)Memory.GetReg(reg2)));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        // Z  < reg1> | < reg2> bitwise OR operation; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        private static void ORR(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(9, ((byte)Memory.GetReg(reg1) | (byte)Memory.GetReg(reg2)));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        //Z <- < reg1> ^ < reg2> bitwise Exclusive OR operation; 
        //1 byte op code | 1 byte register | 1 byte register | 3 bytes unused
        //there is little actual need for this instruction 
        private static void EOR(int page)
        {
            int reg1 = Memory.GetMem(Memory.PC(), page);
            int reg2 = Memory.GetMem(Memory.PC(), page);
            Memory.SetReg(9, ((byte)Memory.GetReg(reg1) ^ (byte)Memory.GetReg(reg2)));
            Memory.increasePC();
            Memory.increasePC();
            Memory.increasePC();
        }

        //Interrupts
        //Execute interrupt <imm>; 
        //1 byte op code | 4 byte immediate | 1 byte unused
        private static void SWI(int page)
        {
            //Combind next 4 bytes and increase PC
            byte one = Memory.GetMem(Memory.PC(), page);
            byte two = Memory.GetMem(Memory.PC(), page);
            byte three = Memory.GetMem(Memory.PC(), page);
            byte four = Memory.GetMem(Memory.PC(), page);
            //load value into PC or 11
            int interrupt = AppendBytes(one, two, three, four);

            switch (interrupt)
            {
                //Standard input
                case 0:
                    var input = Console.Read();
                    Memory.SetMem(Memory.FP(), page, (byte)input);
                    break;
                //Standard output
                case 1:
                    Console.WriteLine("{0}", Memory.GetMem(Memory.FP(), page));
                    break;
                //2 - Standard error 
                case 2:
                    
                    break;
                default:
                    Console.WriteLine("Error: No SWI command found");
                    break;
            }// end Switch
            Memory.increasePC();
        }

        // Take Four byts and return and int
        private static int AppendBytes(byte one, byte two, byte thee, byte four)
        {
            int combined = one << two << thee << four;
            //int combined = four >> thee >> two >> one;
            return combined;
        }
    }
}
