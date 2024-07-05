using Godot;
using System;
using System.Diagnostics;

public static class aiMessageDoc
{
	public static string message = "The Relatively Simple CPU is an 8-bit processor with a 64K address space. " +
            "It interfaces to memory and I/O devices via a 16-bit address bus and an 8-bit system data bus. " +
            "The Relatively Simple CPU uses memory-mapped I/O, so only Read and Write signals are included in the system’s control bus. " +
            "(Other control signals found in some CPUs, such as a READY signal, were excluded to simplify the presentation of the processor.) " +
            "The instruction set architecture of the Relatively Simple CPU includes three registers that can be controlled directly by the programmer. " +
            "The accumulator, AC, is an 8-bit register. It receives the result of any arithmetic or logical operation and provides one of the operands for arithmetic and logical instructions that use two operands. " +
            "Whenever data is loaded from memory, it is loaded into the accumulator; data stored to memory also comes from AC. Register R is an 8-bit general purpose register. " +
            "It supplies the second operand of all 2-operand arithmetic and logical instructions and can also be used to store data. " +
            "Finally, there is a 1-bit zero flag, Z, which is set whenever an arithmetic or logical instruction is executed. " +
            "There are several other registers in this CPU which are not a part of the instruction set architecture, but which the CPU uses to perform the internal operations necessary to fetch, decode, and execute instructions. " +
            "These registers are fairly standard, and are found in many CPUs. The Relatively Simple CPU contains the following registers. " +
            "A 16-bit Address Register, AR, which supplies an address to memory via address pins A[15..0] " +
            "A 16-bit Program Counter, PC, which contains the address of the next instruction to be executed or the address of the next required operand of the instruction " +
            "An 8-bit Data Register, DR, which receives instructions and data from memory and transfers data to memory via data pins D[7..0] " +
            "An 8-bit Instruction Register, IR, which stores the opcode fetched from memory An 8-bit Temporary Register, TR, which temporarily stores data during instruction execution " +
            "The registers within the Relatively Simple CPU are connected via a 16-bit internal bus. In addition, there are a few direct connections between some components within the CPU. " +
            "(This was done to allow two data values to be transferred simultaneously.) The internal organization of the register section of the Relatively Simple CPU is shown in the screen shot of the CPU Internal " +
            "The arithmetic/logic unit for the Relatively Simple CPU is designed as two separate sections, one of which processes arithmetic operations and the other for performing logical functions. " +
            "The instruction set for this CPU contains 16 instructions. Although it is possible to encode these instructions using only four bits, this CPU uses an 8-bit opcode. This was done because the instruction set is expanded later in the textbook1 as other topics, such as interrupts, are introduced. " +
            "The instructions were chosen to represent instructions and instruction types commonly found in processors of this level. The instruction set for the Relatively Simple CPU is shown in Table 1. " +
            "The LDAC, STAC, JUMP, JMPZ and JPNZ instructions all require a 16-bit memory address, represented in the instruction code by Γ. Since each byte of memory is 8 bits wide, these instructions each require three bytes in memory. The first byte contains the opcode for the instruction and the last two bytes contain the address. Following the convention used by Intel’s 8085 microprocessor, the second byte contains the low-order 8 bits of the address and the third byte contains the high-order 8 bits of the address. " +
            "The Relatively Simple CPU can use either a hard-wired or microcoded control unit, either of which can be simulated by this package. " +
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
            "NOT: ~AC -> AC" +
            "Result should be based on this example Result:" +
            "double a number.  i=i+i " +
            "i at 100 " +
            "Code Generated example: " +
            "ldac 100 ;load i" +
            "mvac ;copy to R" +
            "add ;i+i" +
            "stac 100 ;store back to i" +
            "end " +
            "Basically it's format is like this: "    +
            "; is a start of a comment" +
            "[InstructionCode] [Value]" +
            "END" +
            "where InstructionCode is all the instructions and Value is the value and code must end in END" +
            "Using the instruction set code me a program according to this request:";
}
