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
            documentationTemplate.DocContentCloser();
        }
        foreach(var premadeDocTemplateList in  _premadeDocTemplateList)
        {
            premadeDocTemplateList.CloseDocument();
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

}
