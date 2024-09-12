using Godot;
using System;
using System.Buffers.Text;
using System.Diagnostics;

public static class aiMessageDoc
{
    /*public static string message =
    "You are provided with system architecture details, a code format, and a user's request. " +
    "Based on this information, generate a program that fulfills the user's request." +
    "\n\n Information on System Architecture:\n" + information +
    "\n\n Code Format:\n" + format +
    "\n\n User's Request:\n" + userRequest +
    "\n\n Your response should:" +
    "\n- Be based solely on the provided system architecture information." +
    "\n- Strictly follow the given code format, using only the instruction set codes defined." +
    "\n- Provide the final code directly, without any additional explanations or steps." +
    "\n- Clearly state if the request cannot be fulfilled with the provided information and format." +
    "\n\nGenerated Code:";*/

    public static string information = "The Relatively Simple CPU is an 8-bit processor with a 64K address space, using a 16-bit address bus and an 8-bit data bus. " +
            "It employs memory-mapped I/O with only Read and Write signals. The CPU has three programmer-controlled registers: " +
            "AC (8-bit accumulator for arithmetic/logic results), R (8-bit general purpose register for second operands), and Z (1-bit zero flag set by arithmetic/logic instructions). " +
            "The CPU also includes several internal registers: AR (16-bit address register), PC (16-bit program counter), DR (8-bit data register), IR (8-bit instruction register), and TR (8-bit temporary register). " +
            "These are connected by a 16-bit internal bus, with some direct connections for simultaneous data transfer. The ALU is split into sections for arithmetic and logic functions. " +
            "The instruction set consists of 16 instructions, using 8-bit opcodes to allow for future expansion. Instructions requiring a 16-bit memory address (e.g., LDAC, STAC, JUMP) use three bytes: " +
            "one for the opcode, two for the address (low-order first). The CPU can be controlled by either a hard-wired or microcoded control unit. " +
            "Numeric constants in source statements default to decimal unless specified otherwise. Supported number formats include binary (e.g., 11000101B), " +
            "decimal (e.g., 364 or 7534D), octal (e.g., 77O), and hexadecimal (e.g., 84H). The assembler supports directives like ORG <address> to set the start address, " +
            "DB <number> to store 1-byte constants, DW <number> to store 2-byte constants, and labels for jumps (e.g., JMP MyLabel)." +
            "These are its instructions: " +
            "Instruction Instruction Code Operation " +
            "NOP 0000 0000 No operation " +
            "LDAC 0000 0001 Γ AC = M[Γ] " +
            "STAC 0000 0010 Γ M[Γ] = AC " +
            "MVAC 0000 0011 R = AC " +
            "MOVR 0000 0100 AC = R " +
            "JUMP 0000 0101 Γ Goto Γ " +
            "JMPZ 0000 0110 Γ IF (Z=1) THEN Goto Γ " +
            "JPNZ 0000 0111 Γ IF (Z=0) THEN Goto Γ " +
            "ADD 0000 1000 AC = AC + R, If (AC + R = 0) Then Z = 1 Else Z = 0 " +
            "SUB 0000 1001 AC = AC - R, If (AC - R = 0) Then Z = 1 Else Z = 0 " +
            "INAC 0000 1010 AC = AC + 1, If (AC + 1 = 0) Then Z = 1 Else Z = 0 " +
            "CLAC 0000 1011 AC = 0, Z = 1 " +
            "AND 0000 1100 AC = AC & R, If (AC & R = 0) Then Z = 1 Else Z = 0 " +
            "OR 0000 1101 AC = AC | R, If (AC | R = 0) Then Z = 1 Else Z = 0 " +
            "XOR 0000 1110 AC = AC ^ R, If (AC ^ R = 0) Then Z = 1 Else Z = 0 " +
            "NOT 0000 1111 AC = ~AC, If (~AC = 0) Then Z = 1 Else Z = 0 " +
            "Necessary data movement at the beginning before the scan of each instruction:" +
            "Fetch 1: PC -> AR" +
            "Fetch 2: M -> DR, PC+1 -> PC" +
            "Fetch 3: DR -> IR, PC -> AR" +
            "--START OF SCAN INSTRUCTION--" +
            "For LDAC Instruction:" +
            "LDAC 1: M -> DR, PC+1 -> PC, AR+1 -> AR" +
            "LDAC 2: DR -> TR, M -> DR, PC+1 -> PC" +
            "LDAC 3: DR | TR -> AR" +
            "LDAC 4: M -> DR" +
            "LDAC 5: DR -> AC" +
            "For STAC Instruction:" +
            "STAC 1: M -> DR, PC+1 -> PC, AR+1 -> AR" +
            "STAC 2: DR -> TR, M -> DR, PC+1 -> PC" +
            "STAC 3: DR | TR -> AR" +
            "STAC 4: AC -> DR" +
            "STAC 5: DR -> M" +
            "For Other Instructions:" +
            "MVAC: AC -> R" +
            "MOVR: R -> AC" +
            "JUMP: jump to memory location" +
            "JMPZ: if Z = 1 jump to memory location" +
            "JPNZ: if Z = 0 jump to memory location" +
            "NOP: No Operation" +
            "END: End Animation" +
            "ADD:  AC + R -> AC" +
            "SUB: AC - R -> AC" +
            "INAC: AC + 1 -> AC" +
            "CLAC: 0 -> AC, 1 -> Z" +
            "AND: AC & R -> AC" +
            "OR: AC | R -> AC" +
            "XOR: AC ^ R -> AC" +
            "NOT: ~AC -> AC"+
            "I/O port is at ffffh or 65535";

    public static string format =
            "Necessary data movement at the beginning before the scan of each instruction:" +
            "Fetch 1: PC -> AR" +
            "Fetch 2: M -> DR, PC+1 -> PC" +
            "Fetch 3: DR -> IR, PC -> AR" +
            "--START OF SCAN INSTRUCTION--" +
            "For LDAC Instruction:" +
            "LDAC 1: M -> DR, PC+1 -> PC, AR+1 -> AR" +
            "LDAC 2: DR -> TR, M -> DR, PC+1 -> PC" +
            "LDAC 3: DR | TR -> AR" +
            "LDAC 4: M -> DR" +
            "LDAC 5: DR -> AC" +
            "For STAC Instruction:" +
            "STAC 1: M -> DR, PC+1 -> PC, AR+1 -> AR" +
            "STAC 2: DR -> TR, M -> DR, PC+1 -> PC" +
            "STAC 3: DR | TR -> AR" +
            "STAC 4: AC -> DR" +
            "STAC 5: DR -> M" +
            "For Other Instructions:" +
            "MVAC: AC -> R" +
            "MOVR: R -> AC" +
            "JUMP: jump to memory location" +
            "JMPZ: if Z = 1 jump to memory location" +
            "JPNZ: if Z = 0 jump to memory location" +
            "NOP: No Operation" +
            "END: End Animation" +
            "ADD:  AC + R -> AC" +
            "SUB: AC - R -> AC" +
            "INAC: AC + 1 -> AC" +
            "CLAC: 0 -> AC, 1 -> Z" +
            "AND: AC & R -> AC" +
            "OR: AC | R -> AC" +
            "XOR: AC ^ R -> AC" +
            "NOT: ~AC -> AC" +
            "I/O port is at ffffh or 65535" +
            "Numeric constants in source statements default to decimal unless specified otherwise. " +
            "Supported number formats include binary (e.g., 11000101B), decimal (e.g., 364 or 7534D), octal (e.g., 77O), and hexadecimal (e.g., 84H). " +
            "The assembler supports directives like ORG <address> to set the start address, DB <number> to store 1-byte constants, DW <number> to store 2-byte constants, and labels for jumps (e.g., JMP MyLabel)" +
            "Result should be based on these example Results:" +
            "double a number.  i=i+i " +
            "i at 100 " +
            "Generated program: " +
            "ldac 100                                ;load i" +
            "mvac                                    ;copy to R" +
            "add                                     ;i+i" +
            "stac 100                                ;store back to i" +
            "end \n\n" +
            "IF example"+
            "if x+y==0 then I/O port = 0 else I/O port = 1"+
            "x at 100. y at 101"+
            "Generated program: "+
            "ldac 101                               ;load y\r\nadd\t\r\n   ;if not equal to 0 goto Else:\r\n" +
            "jpnz 15                                ;skip to !=0 case (else part)\r\n" +
            ";Then:\r\n" +
            "clac\t\t                               ;0\r\n" +
            "jump 17\t                              ;jump over else part to Endif:\r\n" +
            "                                       ;Else:\r\n" +
            "clac\r\n" +
            "inac\t\t                               ;1\r\n " +
            "                                       ;Endif:\r\n" +
            "stac 65535\r\n" +
            "end" +
            "example from online Help"+
            "Generated program: " +
            "org \t0\t                              ;assembly start address\r\n" +
            "db 2\t\t                               ;defines a byte of decimal value 2\r\n" +
            "db 4                                   ;defines a byte of decimal value 4\r\n" +
            "org 10                                 ;assembly start address\r\n" +
            "ldac 0                                 ;load AC with data at location 0\r\n" +
            "mvac                                   ;move AC data to register R\r\n" +
            "ldac 1                                 ;load AC with data at location 1\r\n" +
            "add                                    ;store the sum of AC, R in AC\r\n" +
            "stac 2                                 ;store AC at location 2\r\n" +
            "stac ffffh                             ;send AC to I/O Port\r\n" +
            "end                                    ;stop" +
            "test logic operations\r\n15=00001111 in 100\r\n85=01010101 in 101"+
            "Generated program: " +
            "ldac\t100\r\n" +
            "mvac\r\n" +
            "ldac\t101\r\n" +
            "and\r\n" +
            "stac\t102\t                            ;AND in 102\r\n" +
            "ldac\t101\r\n" +
            "or\r\n" +
            "stac\t103\t                            ;OR in 103\r\n" +
            "ldac\t101\r\n" +
            "xor\r\n" +
            "stac\t104\t                            ;XOR in 104\r\n" +
            "ldac\t101\r\n" +
            "not\t\r\n" +
            "stac 105\t                             ;NOT in 105\r\nend"+
            "loop x times  x value in 100" +
            "Generated program: " +
            "clac\t\t                               ;i=0\r\n" +
            "mvac\t\t                               ;R=i\r\n" +
            "                                       ;Loop:\r\n" +
            "movr\t\t                               ;AC=i\r\n" +
            "inac\t\t                               ;i++\r\n" +
            "mvac\t\t                               ;i back to R\r\n" +
            "ldac\t100\t                            ;load x\r\n" +
            "sub\t\t                                ;x-i\r\n" +
            "jpnz\t2\t                              ;loop again if i<x\r\n" +
            "end\r\n\r\n; set breakpoint at 9 (jpnz) to count the loops..."+
            "The programs generated must end in END";
            
    public static string userRequest = "";

}
