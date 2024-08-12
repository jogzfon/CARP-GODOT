using Godot;
using System;

public partial class SubstringRange
{
    private int beginIndex;
    private int endIndex;

    public SubstringRange()
    {
        beginIndex = 0;
        endIndex = 0;
    }

    public SubstringRange(int begin, int end)
    {
        SetBeginIndex(begin);
        SetEndIndex(end);
    }

    public SubstringRange(SubstringRange range)
    {
        SetBeginIndex(range.beginIndex);
        SetEndIndex(range.endIndex);
    }

    public int GetBeginIndex()
    {
        return beginIndex;
    }

    public int GetEndIndex()
    {
        return endIndex;
    }

    public int GetLength()
    {
        return endIndex - beginIndex;
    }

    public void SetBeginIndex(int begin)
    {
        if (begin > 0)
        {
            beginIndex = begin;
        }
        else
        {
            beginIndex = 0;
        }
    }

    public void SetEndIndex(int end)
    {
        if (end > 0)
        {
            endIndex = end;
        }
        else
        {
            endIndex = 0;
        }

    }
}
