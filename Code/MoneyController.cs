using Godot;
using System;

public partial class MoneyController : Node
{
	[Export] Texture2D offButtonTexture1;
	[Export] Texture2D onButtonTexture1;
	[Export] Texture2D offButtonTexture2;
	[Export] Texture2D onButtonTexture2;
	[Export] Texture2D offButtonTexture3;
	[Export] Texture2D onButtonTexture3;

	[Export] NodePath button1;
	[Export] NodePath button2;
	[Export] NodePath button3;

	public static MoneyController singleton;

	int moneyIndex = 0;

	public int GetResult()
	{
		return moneyIndex;
	}

	public override void _Ready()
	{
		singleton = this;
		GetNode<TextureButton>(button1).Pressed += () => ButtonPressed(1);
		GetNode<TextureButton>(button2).Pressed += () => ButtonPressed(2);
		GetNode<TextureButton>(button3).Pressed += () => ButtonPressed(3);
	}

	Texture2D getButtonTexture(int index, bool on)
	{
		if (index == 1) return on ? onButtonTexture1 : offButtonTexture1;
		if (index == 2) return on ? onButtonTexture2 : offButtonTexture2;
		if (index == 3) return on ? onButtonTexture3 : offButtonTexture3;
		return null;
	}

	void ButtonPressed(int index)
	{
		moneyIndex = index;
		GetNode<TextureButton>(button1).TextureNormal = getButtonTexture(1, index == 1);
		GetNode<TextureButton>(button2).TextureNormal = getButtonTexture(2, index == 2);
		GetNode<TextureButton>(button3).TextureNormal = getButtonTexture(3, index == 3);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
