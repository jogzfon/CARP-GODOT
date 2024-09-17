using Godot;
using System;
using System.Collections.Generic;

public partial class DocumentationManager : Node
{
	[Export] private TextureButton _documentationAdderBtn;
    [Export] private Control _documentationList;
    [Export] private VBoxContainer _premade_documentationBtnList;
    [Export] private VBoxContainer _documentationBtnList;

    [Export] private PackedScene _premadeDocumentationTemplate;

    private List<PremadeDoc> _premadeDocs = new List<PremadeDoc>();

    [ExportCategory("Premade Document 1 : What is this?")]
    [Export] private Godot.Collections.Array<string> pd_sentences1 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images1 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 2 : How does this work?")]
    [Export] private Godot.Collections.Array<string> pd_sentences2 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images2 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 3 : Program Examples")]
    [Export] private Godot.Collections.Array<string> pd_sentences3 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images3 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 4 : Numeric Constants")]
    [Export] private Godot.Collections.Array<string> pd_sentences4 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images4 = new Godot.Collections.Array<Texture2D>();
    [ExportCategory("Premade Document 5 : Assembler Directives")]
    [Export] private Godot.Collections.Array<string> pd_sentences5 = new Godot.Collections.Array<string>();
    [Export] private Godot.Collections.Array<Texture2D> pd_images5 = new Godot.Collections.Array<Texture2D>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        PremadeDoc1();
        PremadeDoc2();
        PremadeDoc3();
        PremadeDoc4();
        PremadeDoc5();

        DistributePremadeDocumentations();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (AccountManager.GetUser() != null)
		{
			if(AccountManager.GetRole().Contains("Teacher") || AccountManager.GetSubscription().Contains("Subscribed"))
			{
				_documentationAdderBtn.Visible = true;
				_documentationList.Visible = true;
				_documentationBtnList.Visible = true;
				
            }
			else
			{
                _documentationAdderBtn.Visible = false;
                _documentationList.Visible = false;
                _documentationBtnList.Visible = false;
            }
		}
		else
		{
            _documentationAdderBtn.Visible = false;
            _documentationList.Visible = false;
            _documentationBtnList.Visible = false;
        }
	}

    #region Premade Documentations
    private void PremadeDoc1()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences1[0]);
        pre_doc.AddTextEdit(pd_sentences1[1]);

        pre_doc.AddTextureRect(pd_images1[0]);

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc2()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences2[0]);

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc3()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences3[0]);

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc4()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences4[0]);

        _premadeDocs.Add(pre_doc);
    }
    private void PremadeDoc5()
    {
        var pre_doc = new PremadeDoc();
        pre_doc.AddTitle(pd_sentences5[0]);

        _premadeDocs.Add(pre_doc);
    }

    #endregion

    private void DistributePremadeDocumentations()
    {
        foreach (PremadeDoc premadeDoc in _premadeDocs)
        {
            if (premadeDoc.elements.Count > 0)
            {
                var marginContainer = premadeDoc.elements[0] as MarginContainer; // Access MarginContainer if it's the parent
                if (marginContainer != null)
                {
                    var titleLabel = marginContainer.GetChild(0) as Label; // Assuming the Label is the first child
                    
                    var btn = new Button
                    {
                        Text = titleLabel.Text
                    };
                    
                    DistributePremadeValuesToTemplate(premadeDoc, btn);
                    
                    _premade_documentationBtnList.AddChild(btn);

                }
            }
        }
    }
    private void DistributePremadeValuesToTemplate(PremadeDoc premadeDoc, Button btn)
    {
        var premadeDocTemplate = (PremadeDocTemplate)_premadeDocumentationTemplate.Instantiate();

        premadeDocTemplate.AddDocumentBtn(btn);

        for (int i = 0; i < premadeDoc.elements.Count; i++)
        {
            premadeDocTemplate.AddContent(premadeDoc.elements[i]);
        }
        
        _documentationList.AddChild(premadeDocTemplate);
    }

}
