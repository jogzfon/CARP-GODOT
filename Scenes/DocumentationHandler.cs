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
            GD.Print("Hi I got called");
            documentationAdder.Visible = !documentationAdder.Visible;
        }
    }
}