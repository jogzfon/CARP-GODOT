using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public partial class DocumentationManager : Node
{
	[Export] private TextureButton _documentationAdderBtn;
    [Export] private Node _documentationList;
    [Export] private VBoxContainer _premade_documentationBtnList;
    [Export] private VBoxContainer _documentationBtnList;

    [Export] private DocumentationAbler _docAbler;

    [Export] private PackedScene _premadeDocumentationTemplate;

    private List<PremadeDoc> _premadeDocs = new List<PremadeDoc>();

    #region Premade Docs
    [ExportCategory("Premade Document 1 : What is this?")]
    [Export] private Godot.Collections.Array<string> pd_sentences1 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images1 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 2 : How does this work?")]
    [Export] private Godot.Collections.Array<string> pd_sentences2 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images2 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 3 : Program Examples")]
    [Export] private Godot.Collections.Array<string> pd_sentences3 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images3 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 4 : Numeric Constants")]
    [Export] private Godot.Collections.Array<string> pd_sentences4 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images4 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 5 : Assembler Directives")]
    [Export] private Godot.Collections.Array<string> pd_sentences5 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images5 = new Godot.Collections.Array<Texture2D>();

    #endregion
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        #region Premade Docs
        PremadeStrings1();
        PremadeStrings2();
        PremadeStrings3();
        PremadeStrings4();
        PremadeStrings5();

        PremadeDoc1();
        PremadeDoc2();
        PremadeDoc3();
        PremadeDoc4();
        PremadeDoc5();


        DistributePremadeDocumentations();
        #endregion
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if (AccountManager.GetUser() != null)
		{
			if(AccountManager.GetRole().Contains("Teacher") || AccountManager.GetSubscription().Contains("Subscribed"))
			{
				_documentationAdderBtn.Visible = true;
				_documentationBtnList.Visible = true;
				
            }
			else
			{
                _documentationAdderBtn.Visible = false;
                _documentationBtnList.Visible = false;
            }
		}
		else
		{
            _documentationAdderBtn.Visible = false;
            _documentationBtnList.Visible = false;
        }
	}

    #region Premade Documentations
    private void PremadeDoc1()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences1[0]);
        pre_doc.AddTextEdit(pd_sentences1[1]);

        pre_doc.AddTextureRect(pd_images1[0]);

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc2()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences2[0]);

        foreach(Texture2D text2D in pd_images2)
        {
            pre_doc.AddTextureRect(text2D);
        }
        pre_doc.AddTextEdit(pd_sentences2[1]);
        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc3()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences3[0]);

        for (int i = 1; i <pd_sentences3.Count; i++)
        {
            pre_doc.AddTextEdit(pd_sentences3[i]);
        }

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc4()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences4[0]);

        for (int i = 1; i < pd_sentences4.Count; i++)
        {
            pre_doc.AddTextEdit(pd_sentences4[i]);
        }

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc5()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences5[0]);

        for (int i = 1; i < pd_sentences5.Count; i++)
        {
            if (i != 3)
            {
                pre_doc.AddTextEdit(pd_sentences5[i]);
            }
            else
            {
                pre_doc.AddTitle(pd_sentences5[i]);
            }
            
        }

        _premadeDocs.Add(pre_doc);
    }

    private void PremadeStrings1()
    {
        string description = "The Relatively Simple CPU Simulator is an instructional aid for students studying " +
        "Computer Organization and Architecture, typically at the junior or senior level. It simulates the Relatively " +
        "Simple CPU based on John D. Carpinelli's Relatively Simple Computer System Simulator.\n\n" +

        "Students first enter an assembly language program, which is assembled by the simulator. After correcting " +
        "any syntax errors, the user simulates the fetch, decode, and execute cycles for each instruction in the " +
        "program. The user may simulate the execution of the program by clock cycle, by instruction, using breakpoints, " +
        "or as a single, continuous execution.\n\n" +

        "The Relatively Simple Computer System simulator is an instructional aid for teaching computer system design. " +
        "It allows the user to simulate the flow of data within a computer consisting of the Relatively Simple CPU, " +
        "memory, and a memory-mapped I/O port, as it fetches, decodes, and executes instructions. It uses animation " +
        "to illustrate the flow of data between components.\n\n" +

        "The Relatively Simple CPU can access 64K bytes of memory, each byte being 8 bits wide. The CPU does this by " +
        "outputting a 16-bit address on its output pins A[15..0] and reading in the 8-bit value from memory on its " +
        "inputs D[7..0].\n\n" +

        "This CPU has two programmer-accessible registers: an 8-bit accumulator labeled AC, and an 8-bit general " +
        "purpose register R. It also has a 1-bit zero flag, Z. It has sixteen instructions in its instruction set.";


        pd_sentences1.Add(description);
    }
    private void PremadeStrings2()
    {
        pd_sentences2.Add("ANIMATION CONTROLS\n" +
        "START\t\t\t\t\t\t\t\t\t\t\t\t: Begins or resumes the animation.\n" +
        "STOP\t\t\t\t\t\t\t\t\t\t\t\t: Pauses or stops the animation.\n" +
        "Animation Toggle\t\t\t\t\t\t\t: Turns the animation on or off.\n" +
        "Reset Registers\t\t\t\t\t\t\t\t: Resets all registers to their default state.\n" +
        "Step Through Cycle\t\t\t\t\t\t: After starting the animation, click twice to step through each cycle.\n" +
        "                  \t\t\t\t\t\t\t\t\t\t: Clicking it once will play the animation normally.\n" +
        "Step Through Instruction\t\t\t\t: After starting the animation, click twice to step through each instruction.\n" +
        "                        \t\t\t\t\t\t\t\t: Clicking it once will play the animation normally.\n");
    }

    private void PremadeStrings3()
    {
        string example1 = @";double a number.  i=i+i
;i at 100

ldac 100	                                  	  ;load i
mvac		                                      ;copy to R
add		                                          ;i+i
stac 100                                          ;store back to i
end";
        string example2 = @";add two numbers.  x+y   Show overflow.
;x at 100, y at 101, sum at 102

ldac 100	                           		      ;load x
mvac		                                      ;copy to R
ldac 101                           		          ;load y
add		                               	          ;x+y
stac 102                            		      ;store to sum
end";
        string example3 = @";IF example.  
;if x+y==0 then I/O port = 0 else I/O port = 1
;x at 100. y at 101

ldac 100                                          ;load x
mvac
ldac 101                                          ;load y
add	
                                                  ;if not equal to 0 goto Else:
jpnz	 15                                       ;skip to !=0 case (else part)
                                                  ;Then:
clac		                                      ;0
jump 17	                                          ;jump over else part to Endif:
                                                  ;Else:
clac
inac		                                      ;1
                                                  ;Endif:
stac 65535
end";
        string example4 = @";example from online Help
org 	0	                                      ;assembly start address
db 2         	                                  ;defines a byte of decimal value 2
db 4                                              ;defines a byte of decimal value 4
org 10                                            ;assembly start address
ldac 0                                            ;load AC with data at location 0
mvac                                              ;move AC data to register R
ldac 1                                            ;load AC with data at location 1
add	                                              ;store the sum of AC, R in AC
stac 2                                            ;store AC at location 2
stac ffffh                                        ;send AC to I/O Port
end                                               ;stop";
        string example5 = @";example from textbook
;Variables memory locations: total=100  i=101  n=102  
;Label loop=7
;Need to manually Edit Memory location 102 for n value.

clac
stac 100	                                      ;total=0
stac 101	                                      ;i=0
                                                  ;Loop:
ldac 101	                                      ;i++
inac		
stac 101

mvac		                                      ;i to R
ldac 100	                                      ;total to AC
add		                                          ;total+i
stac 100                                          ;total+=i

ldac 102                                          ;n to AC
sub		                                          ;n-i
jpnz 7		                                      ;if n-i!=0 (i.e. i!=n) goto Loop
		                                          ;else continue here
end                                               ;added to book's code.";
        string example6 = @";test logic operations
;15=00001111 in 100
;85=01010101 in 101

ldac	100
mvac
ldac	101
and
stac	102	                                      ;AND in 102
ldac	101
or
stac	103	                                      ;OR in 103
ldac	101
xor
stac	104	                                      ;XOR in 104
ldac	101
not	
stac 105	                                      ;NOT in 105
end";
        string example7 = @";loop x times  x value in 100
clac		         	                          ;i=0
mvac		                                      ;R=i
                       	  		                  ;Loop:
movr		                                      ;AC=i
inac		                                      ;i++
mvac		                                      ;i back to R
ldac	100                             		  ;load x
sub		                          	              ;x-i
jpnz	2	                          		      ;loop again if i<x
end

; set breakpoint at 9 (jpnz) to count the loops...";
        string example8 = @";busy-wait poll of I/O port until nonzero
clac
mvac		                                      ;R=0
          		                                  ;Loop:
ldac	 65535	
sub		                                          ;I/O port value - 0
jmpz 2	                                          ;jump to Loop if I/O port value is 0

ldac 65535		                                  ;assume it hasn't changed...
stac 100		                                  ;put it to ""regular"" memory
end";
        pd_sentences3.Add(example1);
        pd_sentences3.Add(example2);
        pd_sentences3.Add(example3);
        pd_sentences3.Add(example4);
        pd_sentences3.Add(example5);
        pd_sentences3.Add(example6);
        pd_sentences3.Add(example7);
        pd_sentences3.Add(example8);
    }

    private void PremadeStrings4()
    {
        string constraint1 = @"Numeric constants can be used in source statements. If there is no postfix, the assembler assumes the number is decimal.

number can be one of the following:

                   - bin_numB
                   - dec_num (or dec_numD)
                   - oct_numO (or oct_numQ)
                   - hex_numH

                   Lowercase equivalences are allowed: b, d, o, q, h";
        string constraint2 = @"bin_num is a binary number consisting of the digits '0'-'1' and ending with a 'B' or 'b'.

Examples:
                   11000101B
                   1011B
                   1110110b";
        string constraint3 = @"dec_num is a decimal number consisting of the digits '0'-'9', optionally followed by 'D' or 'd'.

Examples:
                   364
                   7534D
                   435d";
        string constraint4 = @"oct_num is an octal number consisting of the digits '0'-'7' and ending with an 'O', 'o', 'Q' or 'q'.

Examples:
                   77O
                   542o
                   2324q
                   34241Q";
        string constraint5 = @"hex_num is a hexadecimal number consisting of '0'-'9' and 'a'-'f' or 'A'-'F' ending with a 'H', or 'h'.

Examples:
                   84H
                   ABDEh
                   f12aH";

        pd_sentences4.Add(constraint1);
        pd_sentences4.Add(constraint2);
        pd_sentences4.Add(constraint3);
        pd_sentences4.Add(constraint4);
        pd_sentences4.Add(constraint5);
    }

    private void PremadeStrings5()
    {
        string assembly1 = @"The assembly process can be controlled by assembler directives. The assembler interprets the assembler directives instead of translating them into machine instructions. The assembler supports the following directives:";
        string assembly2 = @"Directive                          Description

ORG <address>  Assemble the subsequent source statements starting at the specified address, 
                              <address>.

                              Examples:
                                               ORG 1123H ; Start assembling at address 4387
                                               ORG 567o ; Start assembling at address 375

DB <number>     Store the specified 1-byte constant <number> in memory.

                              Examples:
                                               DB 11 ; Store the 8-bit constant 11 in memory
                                               DB 333o ; Store the 8-bit constant 219 in memory
DW <number>    Store the specified 2-byte constant <number> in memory. The assembler initializes the 
                              memory with the least significant byte first.

                              Examples:
                                               DW 23785 ; Store the 16-bit constant 23785 in memory
                                               DW af43h ; Store the 16-bit constant 44867 in memory";
        string assembly3 = "Assembler Labels";
        string assembly4 = @"The assembler also supports jump to lables of the form label_name: (notice the : after the label name). The labels are not case censetive.

Example:
                 JMP MyLabel
                  ....
                  MyLabel: ....";

        pd_sentences5.Add(assembly1);
        pd_sentences5.Add(assembly2);
        pd_sentences5.Add(assembly3);
        pd_sentences5.Add(assembly4);
    }

    private void DistributePremadeDocumentations()
    {
        foreach (PremadeDoc premadeDoc in _premadeDocs)
        {
            if (premadeDoc.elements.Count > 0)
            {
                var marginContainer = premadeDoc.elements[0] as MarginContainer; // Access MarginContainer if it's the parent
                if (marginContainer != null)
                {
                    var titleLabel = marginContainer.GetChild(0) as Label; // Assuming the Label is the first child
                    
                    var btn = new Button
                    {
                        Text = titleLabel.Text
                    };
                    
                    DistributePremadeValuesToTemplate(premadeDoc, btn);
                    
                    _premade_documentationBtnList.AddChild(btn);

                }
            }
        }
    }
    private void DistributePremadeValuesToTemplate(PremadeDoc premadeDoc, Button btn)
    {
        var premadeDocTemplate = (PremadeDocTemplate)_premadeDocumentationTemplate.Instantiate();

        premadeDocTemplate.SetDocumentationAbler(_docAbler);
        premadeDocTemplate.AddDocumentBtn(btn);

        for (int i = 0; i < premadeDoc.elements.Count; i++)
        {
            premadeDocTemplate.AddContent(premadeDoc.elements[i]);
        }

        _docAbler.AddPremadeDocTemplate(premadeDocTemplate);

        _documentationList.AddChild(premadeDocTemplate);
    }
    #endregion
}
