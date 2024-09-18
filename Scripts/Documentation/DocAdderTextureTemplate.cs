using Godot;
using System;

public partial class DocAdderTextureTemplate : HBoxContainer
{
    [Export] private Button _trashBtn;
    [Export] private TextureRect _rect;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _trashBtn.Connect("pressed", new Callable(this, nameof(DeleteImage)));
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        
    }
    public void SetImage(Texture2D img)
    {
        _rect.Texture = img;
        var textureSize = _rect.Texture.GetSize();  // Get the size of the loaded texture
        if (textureSize.X < 1400)
        {
            _rect.ExpandMode = TextureRect.ExpandModeEnum.KeepSize;
            _rect.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            _rect.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            _rect.CustomMinimumSize = new Vector2(100, 100);
        }
        else
        {
            _rect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            _rect.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            _rect.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            var maxWidth = 1400f;
            var maxHeight = 1000f;
            // Calculate scaling factor to maintain aspect ratio within max width and height
            var scaleFactor = Math.Min(maxWidth / textureSize.X, maxHeight / textureSize.Y);

            // Apply the scaling factor to both dimensions to preserve aspect ratio
            _rect.CustomMinimumSize = new Vector2(textureSize.X * scaleFactor, textureSize.Y * scaleFactor);
        }
    }
    public void DeleteImage()
    {
        var parent = this.GetParent();
        if (parent != null)
        {
            parent.QueueFree(); // Free the parent node
        }
    }
    public Texture2D GetTexture()
    {
        return _rect.Texture;
    }

}
