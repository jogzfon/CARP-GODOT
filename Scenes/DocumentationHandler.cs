using Godot;
using System;

public partial class DocumentationHandler : TextureButton
{
	[Export] TextureButton addDocumentationBtn;
    [Export] PanelContainer documentationAdder;

    public override void _Ready()
    {
        addDocumentationBtn.Connect("pressed", new Callable(this, nameof(OpenDocumentationTemplate)));
    }

    private void OpenDocumentationTemplate() {
       if(documentationAdder != null)
        {
            if(documentationAdder.Visible)
            {
                documentationAdder.Visible = false;
            }else
            {
                documentationAdder.Visible = true;
            }
        }
    }
}
