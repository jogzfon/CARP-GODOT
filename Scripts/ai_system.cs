using Godot;
using OpenAI_API.Completions;
using OpenAI_API;
using System;

public partial class ai_system : Control
{
    VBoxContainer chatContainer;
    TextEdit request;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        chatContainer = GetNode<VBoxContainer>("ScrollContainer/ChatContainer");
        request = GetNode<TextEdit>("RequestEdit");

        var sendRequest = GetNode<Button>("SendRequest");
        sendRequest.Connect("pressed", new Callable(this, nameof(Generate_Code)));
        var back = GetNode<TextureButton>("Back");
        sendRequest.Connect("pressed", new Callable(this, nameof(BackToMain)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
    private void BackToMain() {
        Node simultaneousScene = ResourceLoader.Load<PackedScene>("res://Scenes/project_page.tscn").Instantiate();
        GetTree().Root.AddChild(simultaneousScene);
        Hide();
    }
    private async void Generate_Code()
    {
        Label label = new Label();
        label.Text = request.Text;
        label.HorizontalAlignment = HorizontalAlignment.Right;
        label.Set("theme_override_fonts/font", "res://Fonts/Comfortaa-Bold.ttf");

        chatContainer.AddChild(label);
        request.Text = "";

        string info = "The Relatively Simple CPU is an 8-bit processor with a 64K address space. " +
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
            "double a number.  i=i+i " +
            "i at 100 " +
            "ldac 100 ;load i" +
            "mvac ;copy to R" +
            "add ;i+i" +
            "stac 100 ;store back to i" +
            "end " +
            "From the above, using the instruction set, code me a program that will do the request below: ";

        // Input prompt for generating code
        // Your OpenAI API key
        string apiKey = "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";
        APIAuthentication aPIAuthentication = new APIAuthentication(apiKey);
        OpenAIAPI openAiApi = new OpenAIAPI(aPIAuthentication);
        // Prepare the request data
        try
        {
            string prompt = info + "\n" + request.Text;
            string model = "gpt-3.5-turbo-instruct";
            int maxTokens = 200;

            var completionRequest = new CompletionRequest
            {
                Prompt = prompt,
                Model = model,
                MaxTokens = maxTokens
            };

            CompletionResult completionResult = await openAiApi.Completions.CreateCompletionAsync(completionRequest);
            string generatedText = completionResult.Completions[0].Text;

            // Create a new Label
            TextEdit label2 = new TextEdit();
            label2.Text = generatedText;
            label2.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            label2.WrapMode = TextEdit.LineWrappingMode.Boundary;
            label2.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            label2.ScrollFitContentHeight = true;
            label2.Set("theme_override_fonts/font", "res://Fonts/Comfortaa-Bold.ttf");
            // Add label to ListBox
            chatContainer.AddChild(label2);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
