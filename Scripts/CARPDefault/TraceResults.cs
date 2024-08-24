using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class TraceResults
{ 

    static List<Results> results = new List<Results>();
    public static List<string> resultsStatements = new List<string>();


    public static void AddResult(string rtl, string datamove, int ar, int pc, int dr, int tr, int ir, int r, int ac, int z)
    {
        results.Add(new Results(rtl, datamove, ar, pc, dr, tr, ir, r, ac, z));
    }

    public static List<string> GetAllStatements()
    {
        foreach (Results r in results)
        {
            resultsStatements.Add(r.ToString());
        }
        return resultsStatements;
    }
    public static void RemoveAllStatements()
    {
        results.Clear();
        resultsStatements.Clear();
    }

    public static void UpdateTraceResults(TextEdit trace)
    {
        trace.Clear();
        if(results.Count == 0)
        {
            trace.Text = "No results to trace....";
        }
        else
        {
            trace.Text = "Trace Results: " + "\n";
            foreach (Results r in results)
            {
                trace.Text += r.ToString() + "\n";
            }
            trace.Text += "\n";
        }
    }
    public static string getTextTrace()
    {
        string traces = "";
        if (results.Count == 0)
        {
            traces = "No results to trace....";
        }
        else
        {
            traces = "Trace Results: " + "\n";
            foreach (Results r in results)
            {
                traces += r.ToString() + "\n";
            }
            traces += "\n";
        }

        return traces;
    }
}
class Results
{
    private string rtl = "";
    private string datamove = "";
    private int ar = 0x00000000;
    private int pc = 0x00000000;
    private int dr = 0x00000000;
    private int tr = 0x00000000;
    private int ir = 0x00000000;
    private int r = 0x00000000;
    private int ac = 0x00000000;
    private int z = 0;
    public Results(string rtl, string datamove, int ar, int pc, int dr, int tr, int ir, int r, int ac, int z)
    {
        this.rtl = rtl;
        this.datamove = datamove;
        this.ar = ar;
        this.pc = pc;
        this.dr = dr;
        this.tr = tr;
        this.ir = ir;
        this.r = r;
        this.ac = ac;
        this.z = z;
    }
    public override string ToString()
    {
        const int columnWidth = 9; // Adjust the width based on your preference

        string formattedString = $"RTL: {rtl.PadRight(columnWidth)}, " + "\n" +
                         $"DataMove: {datamove.PadRight(columnWidth)}, " + "\n" +
                         $"AR: {SpaceInserter(ar, "ar")}, " +
                         $"PC: {SpaceInserter(pc, "pc")}, " +
                         $"DR: {SpaceInserter(dr, "dr")}, " +
                         $"TR: {SpaceInserter(tr, "tr")}, " +
                         $"IR: {SpaceInserter(ir, "ir")}, " +
                         $"R: {SpaceInserter(r, "r")}, " +
                         $"AC: {SpaceInserter(ac, "ac")}, " +
                         $"Z: {z}";

        return formattedString;
    }

    public string SpaceInserter(int reg, string regname)
    {
        string binaryString;
        if (regname == "ar" || regname == "pc")
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg, 2).PadLeft(16, '0');
        }
        else if (regname == "z")
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg);
        }
        else
        {
            // Convert the integer to a binary string with spaces every 4 digits
            binaryString = Convert.ToString(reg, 2).PadLeft(8, '0');
        }

        // Insert spaces after every 4 digits
        int groupSize = 4;
        for (int i = groupSize; i < binaryString.Length; i += (groupSize + 1))
        {
            binaryString = binaryString.Insert(i, " ");
        }
        return binaryString;
    }
}
