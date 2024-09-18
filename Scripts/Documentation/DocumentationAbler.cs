using Godot;
using System;
using System.Collections.Generic;

public partial class DocumentationAbler : Node
{
    private List<DocumentationTemplate> _documentationTemplateList = new List<DocumentationTemplate>();
    private List<PremadeDocTemplate> _premadeDocTemplateList = new List<PremadeDocTemplate>();

    [ExportCategory("Documentation Adder")]
    [Export] private Control _documentationAdder;

    public void HideAllDocument()
    {
        if (_documentationAdder != null)
        {
            _documentationAdder.Visible = false;
        }
        foreach(var documentationTemplate in _documentationTemplateList)
        {
            if (documentationTemplate != null) {
                documentationTemplate.DocContentCloser();
            }
            else
            {
                GD.Print("Null or Disposed");
            }
        }
        foreach(var premadeDocTemplateList in  _premadeDocTemplateList)
        {
            if (premadeDocTemplateList != null)
            {
                premadeDocTemplateList.CloseDocument();
            }
            else
            {
                GD.Print("Null or Disposed");
            }
        }
    }
    public void AddDocTemplate(DocumentationTemplate template)
    {
        _documentationTemplateList.Add(template);
    }
    public void AddPremadeDocTemplate(PremadeDocTemplate template)
    {
        _premadeDocTemplateList.Add(template);
    }
    public void RemoveDocTemplate(DocumentationTemplate template)
    {
        _documentationTemplateList.Remove(template);
    }
    public void RemoveAllTemplates()
    {
        _documentationTemplateList.Clear();
    }
}
