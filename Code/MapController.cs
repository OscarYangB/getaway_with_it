using Godot;
using System;

public partial class MapController : Node
{
	[Export] Texture2D offButtonTexture;
	[Export] Texture2D onButtonTexture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node child in GetChildren())
		{
			if (child is TextureButton)
			{
				TextureButton button = (TextureButton)child;
				button.Pressed += () => ButtonPressed(button.Name);
			}
		}
	}

	void ButtonPressed(string name)
	{
		foreach (Node child in GetChildren())
		{
			if (child is TextureButton)
			{
				TextureButton button = (TextureButton)child;
				button.TextureNormal = button.Name == name ? onButtonTexture : offButtonTexture;
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
