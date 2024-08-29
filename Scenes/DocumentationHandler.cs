using Godot;
using System;

public partial class DocumentationHandler : TextureButton
{
    [Export] private TextureButton addDocumentationBtn;
    [Export] private Control documentationAdder;

    public override void _Ready()
    {
        addDocumentationBtn.Pressed += OpenDocumentationTemplate;
    }

    private void OpenDocumentationTemplate()
    {
        if (documentationAdder != null)
        {
            documentationAdder.Visible = !documentationAdder.Visible;
        }
    }
}