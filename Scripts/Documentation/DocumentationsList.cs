using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

public partial class DocumentationsList : VBoxContainer
{
    private string directoryPath = "Carp_Documentation";
    [Export] private VBoxContainer _documentationBtnList;
    [Export] private Node _documentationList;
    [Export] private PackedScene _documentationTemplate;

    private bool _isText = false;
    private string _text = String.Empty;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        /*// Create a new Timer instance or add it via the editor and get a reference to it.
        Timer timer = new Timer();

        // Set the timer's wait time (in seconds).
        timer.WaitTime = 5.0f; // 5 seconds

        // Enable the timer to automatically restart after timing out.
        timer.OneShot = false; // Keeps it repeating

        // Add the timer as a child of the current node.
        AddChild(timer);

        // Connect the timeout signal to a method.
        timer.Connect("timeout", new Callable(this, nameof(OnTimerTimeout)));

        // Start the timer.
        timer.Start();*/
        GetAllCarpdocFilesInDirectory();
    }
    /*private void OnTimerTimeout()
    {
        RefreshAllDocumentFilesInCarp();
    }*/

    public void RefreshAllDocumentFilesInCarp()
    {
        foreach (Node child in _documentationBtnList.GetChildren())
        {
            // Example of processing each child, replace with actual logic
            child.QueueFree();
        }
        foreach (Node child in _documentationList.GetChildren())
        {
            // Example of processing each child, replace with actual logic
            child.QueueFree();
        }
        GetAllCarpdocFilesInDirectory();
    }
    public void GetAllCarpdocFilesInDirectory()
    {
        // Open the directory
        DirAccess dir = DirAccess.Open(directoryPath);
        if (dir == null)
        {
            GD.PrintErr("Failed to open directory: " + directoryPath);
            DirAccess.MakeDirAbsolute(directoryPath);
            return;
        }

        // Iterate through the directory contents
        dir.ListDirBegin(); // Start listing the directory contents

        string fileName = dir.GetNext();
        while (fileName != "")
        {
            // Skip special entries "." and ".."
            if (!dir.CurrentIsDir() && fileName.EndsWith(".carpdoc"))
            {
                string filePath = Path.Combine(dir.GetCurrentDir(), fileName);
                using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
                string content = file.GetAsText();

                DocumentationTemplate doc_template = (DocumentationTemplate)_documentationTemplate.Instantiate();
                DistributeDocumentationValues(content, doc_template);

                _documentationList.AddChild(doc_template);
            }

            fileName = dir.GetNext(); // Get the next file
        }

        dir.ListDirEnd(); // Finish listing the directory contents
    }
    private void DistributeDocumentationValues(string content, DocumentationTemplate doc_template)
    {
        List<List<Tokens>> tokens = new List<List<Tokens>>();

        string pattern = @"\b(Title:|Text:|Image:)\b|""[^""]*""|'[^']*'|\b[\w']+\b|\S|\s+|\n|\r\n";

        string[] lines = content.Split('\n');

        foreach (string line in lines)
        {
            if (!line.Trim().StartsWith("-"))
            {
                tokens.Add(TokenizeDocumentationLines(line.Replace(",", ""), pattern));
            }
        }

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].Count > 0)
            {
                SetDocumentationValues(tokens[i], doc_template);
            }
        }
        if (_text.Length > 0)
        {
            doc_template.AddText(_text);
            _text = "";
        }
    }
    private List<Tokens> TokenizeDocumentationLines(string line, string pattern)
    {
        List<Tokens> tokens = new List<Tokens>();
        MatchCollection matches = Regex.Matches(line, pattern);

        foreach (Match match in matches)
        {
            string token = match.Value;

            if (Regex.IsMatch(token, ":"))
            {
                tokens.Add(new Tokens(TokenType.COLON, token));
            }
            else if (Regex.IsMatch(token, @"\b(Title|Text|Image)\b"))
            {
                tokens.Add(new Tokens(TokenType.LABEL, token));
            }
            else
            {
                tokens.Add(new Tokens(TokenType.VALUE, token));
            }
        }
        return tokens;
    }

    private void SetDocumentationValues(List<Tokens> tokens, DocumentationTemplate doc_template)
    {
        switch (tokens[0].type)
        {
            case TokenType.LABEL:
                if (_text.Length > 0)
                {
                    doc_template.AddText(_text);
                    _text = "";
                }
                SetLabelValues(tokens, doc_template);
                break;
            case TokenType.VALUE:
                for (int i = 0; i < tokens.Count; i++)
                {
                    _text += tokens[i].value;
                }
                _text += "\n";
                break;
            default:
                GD.PrintErr(tokens[0].type + ": " + tokens[0].value + " does not exist!");
                break;
        }
    }

    private void SetLabelValues(List<Tokens> tokens, DocumentationTemplate doc_template)
    {
        switch (tokens[0].value)
        {
            case "Title":
                if (2 < tokens.Count)
                {
                    var btn = new Button();
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        btn.Text += tokens[i].value;
                    }
                    doc_template.DocumentationBtn(btn);
                    doc_template.AddTitle(btn.Text);
                    _documentationBtnList.AddChild(btn);
                }
                break;
            case "Image":
                if (2 < tokens.Count)
                {
                    string img = "";
                    for (int i = 2; i < tokens.Count; i++)
                    {
                        img += tokens[i].value;
                    }
                    doc_template.AddImage(img);
                }
                break;
            case "Text":
                if (2 < tokens.Count)
                {
                    for (int i = 2; i < tokens.Count; i++) // Start from the third token
                    {
                        _text += tokens[i].value;
                    }
                }
                _text += "\n";
                break;
        }
    }
}
