using Godot;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public partial class AssemblySourceProgram
{
    private AssemblySourceLine[] sourceLineArray;
    private string sourceString;

    public AssemblySourceProgram(string source)
    {
        sourceString = source;
        /*using (StringReader reader = new StringReader(source))
        {
            TokenizeAndProcess(reader);
        }*/
        TokenizeAndProcess();
    }
    //private void TokenizeAndProcess(StringReader reader)
    private void TokenizeAndProcess()
    {
        /*List<AssemblySourceLine> lineVector = new List<AssemblySourceLine>();
        string lineString = "";
        int searchIndex = 0;
        int lineNumber = 1;

        try
        {
            int nextToken;

            //Counter
            //int count = 0;
            while ((nextToken = reader.Read()) != -1)
            {
                char currentChar = (char)nextToken;

                //Number Printer
                //Console.Write(count +":"+currentChar +" ");
                //count++;

                if (char.IsWhiteSpace(currentChar) && currentChar == '\n')
                {
                    AssemblySourceLine line = FindSourceLine(lineString, searchIndex, lineNumber);
                    if(line != null)
                    {
                        lineVector.Add(line);
                    }

                    if (line.SourceTokenLength() > 0)
                    {
                        empty = true;
                    }

                    lineString = "";
                    lineNumber++;
                    searchIndex = line.GetSubstringRange().GetEndIndex() + 1;
                }
                else
                {
                    lineString += currentChar;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Caught exception: " + e);
        }

        if (!string.IsNullOrEmpty(lineString))
        {
            lineNumber++;
            lineVector.Add(FindSourceLine(lineString, searchIndex, lineNumber));
        }

        sourceLineArray = lineVector.ToArray();*/
        List<AssemblySourceLine> lineVector = new List<AssemblySourceLine>();
        string pattern = @"^(?!;)([^;\r\n]+)";
        Regex regex = new Regex(pattern, RegexOptions.Multiline);
        int lineNumber = 1;
        MatchCollection matches = regex.Matches(sourceString);
        foreach (Match match in matches)
        {
            string lineString = match.Value.Trim();
            if (!string.IsNullOrWhiteSpace(lineString))
            {
                AssemblySourceLine line = FindSourceLine(lineString, lineNumber);
                if (line != null)
                {
                    lineVector.Add(line);
                    GD.Print(line);
                }
                if (line.SourceTokenLength() > 0)
                {
                    lineNumber++;
                }
            }
        }
        sourceLineArray = lineVector.ToArray();
    }

    public AssemblySourceLine GetSourceLineByLineNumber(int line)
    {
        if ((line > 0) && (line <= sourceLineArray.Length))
        {
            return sourceLineArray[line - 1];
        }
        else
        {
            return null;
        }
    }

    public AssemblySourceLine GetSourceLineByAddress(int address)
    {
        if (address >= 0)
        {
            for (int index = 0; index < sourceLineArray.Length; index++)
            {

                if (address == sourceLineArray[index].GetAddress())
                {
                    return (sourceLineArray[index]);
                }

            }
            //return sourceLineArray.FirstOrDefault(line => line.GetAddress() == address);
            return null;
        }
        else
        {
            return null;
        }
    }

    public void SetSourceLineAddressByLineNumber(int address, int line)
    {
        if ((line > 0) && (line <= sourceLineArray.Length))
        {
            sourceLineArray[line - 1].SetAddress(address);
        }
    }

    public int SourceLineLength()
    {
        return sourceLineArray.Length;
    }

    /* private AssemblySourceLine FindSourceLine(string sourceLine, int searchIndex, int line)
     {
         int beginIndex = sourceString.IndexOf(sourceLine, searchIndex);
         int endIndex = beginIndex + sourceLine.Length;

         //Console.WriteLine("\n"+sourceLine +": searchIndex :"+ searchIndex +": begin index :"+beginIndex);
         SubstringRange range = new SubstringRange(beginIndex, endIndex);
         return new AssemblySourceLine(sourceString, range, line);
     }*/
    private AssemblySourceLine FindSourceLine(string sourceLine, int line)
    {
        int beginIndex = sourceString.IndexOf(sourceLine);
        int endIndex = beginIndex + sourceLine.Length;

        // Console.WriteLine("\n" + sourceLine + ": searchIndex :" + searchIndex + ": begin index :" + beginIndex);

        SubstringRange range = new SubstringRange(beginIndex, endIndex);
        return new AssemblySourceLine(sourceString, range, line);
    }   
}
