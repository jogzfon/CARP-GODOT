using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class TraceResults
{ 

    public static List<Results> results = new List<Results>();
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
    public static List<Results> getTextTrace()
    {
        return results;
    }
}
