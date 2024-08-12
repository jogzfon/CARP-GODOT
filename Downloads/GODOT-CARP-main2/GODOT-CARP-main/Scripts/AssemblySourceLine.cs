using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class AssemblySourceLine
{
    private string sourceString;
    private SubstringRange sourceSubstringRange;
    private SubstringRange[] sourceTokenArray;
    private int lineNumber;
    private int address;

    public AssemblySourceLine(string source, SubstringRange range, int line)
       : this(source, range, line, -1)
    {
    }

    public AssemblySourceLine(string source, SubstringRange range, int line, int addr)
    {
        sourceString = source;
        sourceSubstringRange = new SubstringRange(range);
        SetLineNumber(line);
        SetAddress(addr);

        /*using (StringReader reader = new StringReader(ToString()))
        {
            List<SubstringRange> tokenList = new List<SubstringRange>();
            int searchIndex = sourceSubstringRange.GetBeginIndex();

            int token;
            while ((token = reader.Read()) != -1)
            {
                char ch = (char)token;

                if (!char.IsWhiteSpace(ch))
                {
                    string sourceToken = ch.ToString();
                    SubstringRange tokenSubstringRange = FindSourceTokenSubstringRange(sourceToken, searchIndex);
                    tokenList.Add(tokenSubstringRange);
                    searchIndex = tokenSubstringRange.GetEndIndex();
                }
            }

            if (tokenList.Count > 0)
            {
                sourceTokenArray = (SubstringRange[])tokenList.ToArray();
            }
            else
            {
                sourceTokenArray = null;
            }
        }*/
        string pattern = @"[^\s]+"; // Pattern to match non-whitespace sequences (words)
        Regex regex = new Regex(pattern);

        List<SubstringRange> tokenVector = new List<SubstringRange>();
        int searchIndex = sourceSubstringRange.GetBeginIndex();

        MatchCollection matches = regex.Matches(ToString());

        foreach (Match match in matches)
        {
            SubstringRange tokenSubstringRange = FindSourceTokenSubstringRange(match.Value, searchIndex);
            tokenVector.Add(tokenSubstringRange);
            searchIndex = tokenSubstringRange.GetEndIndex();
        }

        if (tokenVector.Count > 0)
        {
            sourceTokenArray = tokenVector.ToArray();
        }
        else
        {
            sourceTokenArray = null;
        }
    }

    public override string ToString()
    {
        return sourceString.Substring(sourceSubstringRange.GetBeginIndex(), sourceSubstringRange.GetEndIndex() - sourceSubstringRange.GetBeginIndex());
    }

    public SubstringRange GetSubstringRange()
    {
        return new SubstringRange(sourceSubstringRange);
    }

    public int GetLineNumber()
    {
        return lineNumber;
    }

    public int GetAddress()
    {
        return address;
    }

    public void SetLineNumber(int line)
    {
        lineNumber = line > 0 ? line : 0;
    }

    public void SetAddress(int addr)
    {
        address = addr >= 0 ? addr : -1;
    }

    public string[] GetSourceTokenStringArray()
    {
        if (sourceTokenArray != null)
        {
            string[] returnArray = new string[sourceTokenArray.Length];

            for (int index = 0; index < sourceTokenArray.Length; index++)
            {
                returnArray[index] = sourceString.Substring(
                    sourceTokenArray[index].GetBeginIndex(),
                    sourceTokenArray[index].GetEndIndex() - sourceTokenArray[index].GetBeginIndex());
            }

            return returnArray;
        }
        else
        {
            return null;
        }
    }

    public SubstringRange[] GetSourceTokenSubstringRangeArray()
    {
        if (sourceTokenArray != null)
        {
            SubstringRange[] returnArray = new SubstringRange[sourceTokenArray.Length];
            Array.Copy(sourceTokenArray, returnArray, sourceTokenArray.Length);
            return returnArray;
        }
        else
        {
            return null;
        }
    }

    public int SourceTokenLength()
    {
        return sourceTokenArray != null ? sourceTokenArray.Length : 0;
    }

    public bool ContainsLabel()
    {
        string[] tokens = GetSourceTokenStringArray();
        return IsLabel(tokens[0]);
    }

    public string GetLabel()
    {
        if (GetSourceTokenStringArray() != null)
        {
            string[] tokens = GetSourceTokenStringArray();
            return IsLabel(tokens[0]) ? tokens[0] : string.Empty;
        }
        return string.Empty;
    }

    private bool IsLabel(string token)
    {
        return token.IndexOf(':') != -1 && token.IndexOf(':') == token.Length - 1 && token.Length > 1;
    }

    private SubstringRange FindSourceTokenSubstringRange(string sourceToken, int searchIndex)
    {
        int beginIndex = sourceString.IndexOf(sourceToken, searchIndex);
        int endIndex = beginIndex + sourceToken.Length;

        return new SubstringRange(beginIndex, endIndex);
    }
}
