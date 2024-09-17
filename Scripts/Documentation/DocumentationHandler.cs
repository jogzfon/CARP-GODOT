using Godot;
using System;

public partial class DocumentationHandler : TextureButton
{
    [Export] private TextureButton addDocumentationBtn;
    [Export] private Control documentationAdder;

    [ExportCategory("Documentation Disabler")]
    [Export] private DocumentationAbler _documentationAbler;

    [ExportCategory("File System Buttons")]
    [Export] private Button newProj;
    [Export] private Button openProj;

    public override void _Ready()
    {
        addDocumentationBtn.Connect("pressed", new Callable(this, nameof(OpenDocumentationTemplate)));
    }
    public override void _Process(double delta)
    {
        if (documentationAdder.Visible)
        {
            newProj.Disabled = true;
            openProj.Disabled = true;
        }
        else
        {
            newProj.Disabled = false;
            openProj.Disabled = false;
        }
    }

    private void OpenDocumentationTemplate()
    {
        _documentationAbler.HideAllDocument();
        if (documentationAdder != null)
        {
            documentationAdder.Visible = !documentationAdder.Visible;
        }
    }
}