using System;
using System.Collections.Generic;
using System.Numerics;

public class PremadeCodeList
{
	public List<PremadeCode> premadeCodes = new List<PremadeCode>();

    public void InitializePremadeCodes()
    {
        premadeCodes.Add(new PremadeCode("EX1",
        ";double a number.  i=i+i\r\n;i at 100\r\n\r\nldac 100\t                                  \t  ;load i\r\nmvac\t\t                                      ;copy to R\r\nadd\t\t                                          ;i+i\r\nstac 100                               \t\t  ;store back to i\r\nend"));
        premadeCodes.Add(new PremadeCode("EX2",
        ";add two numbers.  x+y   Show overflow.\r\n;x at 100, y at 101, sum at 102\r\n\r\nldac 100\t                           \t\t      ;load x\r\nmvac\t\t                                      ;copy to R\r\nldac 101                           \t\t      ;load y\r\nadd\t\t                               \t         ;x+y\r\nstac 102                            \t\t    ;store to sum\r\nend"));
        premadeCodes.Add(new PremadeCode("EX3",
        ";IF example.  \r\n;if x+y==0 then I/O port = 0 else I/O port = 1\r\n;x at 100. y at 101\r\n\r\nldac 100                                ;load x\r\nmvac\r\nldac 101                                ;load y\r\nadd\t\r\n                                               ;if not equal to 0 goto Else:\r\njpnz\t 15                                  ;skip to !=0 case (else part)\r\n                                              ;Then:\r\nclac\t\t                                       ;0\r\njump 17\t                                ;jump over else part to Endif:\r\n                                              ;Else:\r\nclac\r\ninac\t\t                                       ;1\r\n                                              ;Endif:\r\nstac 65535\r\nend"));
        premadeCodes.Add(new PremadeCode("EX4",
        ";example from online Help\r\norg \t0\t                                      ;assembly start address\r\ndb 2         \t                              ;defines a byte of decimal value 2\r\ndb 4                                       ;defines a byte of decimal value 4\r\norg 10                                    ;assembly start address\r\nldac 0                                    ;load AC with data at location 0\r\nmvac                                      ;move AC data to register R\r\nldac 1                                     ;load AC with data at location 1\r\nadd\t                                         ;store the sum of AC, R in AC\r\nstac 2                                     ;store AC at location 2\r\nstac ffffh                                ; send AC to I/O Port\r\nend                                         ;stop"));
        premadeCodes.Add(new PremadeCode("EX5",
        ";example from textbook\r\n;Variables memory locations: total=100  i=101  n=102  \r\n;Label loop=7\r\n;Need to manually Edit Memory location 102 for n value.\r\n\r\nclac\r\nstac 100\t                                           ;total=0\r\nstac 101\t                                            ;i=0\r\n                                                          ;Loop:\r\nldac 101\t                                            ;i++\r\ninac\t\t\r\nstac 101\r\n\r\nmvac\t\t                                                ;i to R\r\nldac 100\t                                          ;total to AC\r\nadd\t\t                                                  ;total+i\r\nstac 100                                         \t;total+=i\r\n\r\nldac 102                                         \t;n to AC\r\nsub\t\t                                            ;n-i\r\njpnz 7\t\t                                        ;if n-i!=0 (i.e. i!=n) goto Loop\r\n\t\t                                                ;else continue here\r\nend                                                  ;added to book's code."));
        premadeCodes.Add(new PremadeCode("EX6",
        ";test logic operations\r\n;15=00001111 in 100\r\n;85=01010101 in 101\r\n\r\nldac\t100\r\nmvac\r\nldac\t101\r\nand\r\nstac\t102\t                                    ;AND in 102\r\nldac\t101\r\nor\r\nstac\t103\t                                    ;OR in 103\r\nldac\t101\r\nxor\r\nstac\t104\t                                    ;XOR in 104\r\nldac\t101\r\nnot\t\r\nstac 105\t                                   ;NOT in 105\r\nend"));
        premadeCodes.Add(new PremadeCode("EX7",
        ";loop x times  x value in 100\r\nclac\t\t         \t                                    ;i=0\r\nmvac\t\t                                           ;R=i\r\n                       \t  \t\t                           ;Loop:\r\nmovr\t\t                                           ;AC=i\r\ninac\t\t                                               ;i++\r\nmvac\t\t                                           ;i back to R\r\nldac\t100                             \t\t         \t;load x\r\nsub\t\t                          \t                    ;x-i\r\njpnz\t2\t                          \t\t                ;loop again if i<x\r\nend\r\n\r\n; set breakpoint at 9 (jpnz) to count the loops..."));
        premadeCodes.Add(new PremadeCode("EX8",
        ";busy-wait poll of I/O port until nonzero\r\nclac\r\nmvac\t\t                                        ;R=0\r\n          \t\t                                        ;Loop:\r\nldac\t 65535\t\r\nsub\t\t                                            ;I/O port value - 0\r\njmpz 2\t                                        ;jump to Loop if I/O port value is 0\r\n\r\nldac 65535\t\t                                ;assume it hasn't changed...\r\nstac 100\t\t                                    ;put it to \"regular\" memory\r\nend"));
    }
    public (string message, int errNum) AddSetOfCodes(string keyword, string instructions)
    {
        if (KeyWordExists(keyword))
        {
            return ("Keyword '{keyword}' already exists.", 1);
        }
        try
        {
            if(instructions == null || instructions == String.Empty) {
                return ("No instructions found.", 1);
            }

            premadeCodes.Add(new PremadeCode(keyword, instructions));
            PresetCodeFileSaver.SavePresetCode(keyword, instructions); // Save the preset as script
            return ("Preset Code Added Successfully.", 0);
        }
        catch (Exception e)
        {
            return ($"Error adding code: {e.Message}", 2);
        }
    }

    private bool KeyWordExists(string keyword)
    {
        foreach (var code in premadeCodes)
        {
            if (code.KeyWord.Equals(keyword, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
