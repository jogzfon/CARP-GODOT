using Godot;
using System;

public partial class GuideHandler : Node
{
    [Export] public Control guides;
    [Export] public Button guidesButton;
    [Export] public TextureButton nextButton;
    [Export] public TextureButton prevButton;

    private int page_number = 1;
    private int max_pages = 1;
    public override void _Ready()
    {
        guides.Hide();
        guidesButton.Connect("pressed", new Callable(this, nameof(OpenGuide)));
        nextButton.Connect("pressed", new Callable(this, nameof(NextPage)));
        prevButton.Connect("pressed", new Callable(this, nameof(PrevPage)));

        max_pages = guides.GetChildCount()-2;
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (page_number == 1)
        {
            prevButton.Disabled = true;
        }
        else
        {
            prevButton.Disabled = false;
        }
    }
    private void OpenGuide()
    {
        if (guides.Visible)
        {
            guides.Hide();
        }
        else
        {
            guides.Show();
            TextureRect page = guides.GetChild(page_number) as TextureRect;
            page.Show();
        }
    }

    private void NextPage()
    {
        if (page_number == max_pages)
        {
            TextureRect prev_page = guides.GetChild(page_number) as TextureRect;
            prev_page.Hide();

            page_number = 1;
            guides.Hide();
        }
        else
        {
            TextureRect prev_page = guides.GetChild(page_number) as TextureRect;
            prev_page.Hide();
            page_number++;
            TextureRect next_page = guides.GetChild(page_number) as TextureRect;
            next_page.Show();
        }
    }

    private void PrevPage() 
    {
        TextureRect prev_page = guides.GetChild(page_number) as TextureRect;
        prev_page.Hide();
        page_number--;
        TextureRect next_page = guides.GetChild(page_number) as TextureRect;
        next_page.Show();
    }
}
