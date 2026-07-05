using Godot;
using System;
using System.Text.RegularExpressions;

public partial class PeopleController : Node
{
	int count = 0;
	[Export] NodePath upButton;
	[Export] NodePath downButton;
	[Export] NodePath label;

	public static PeopleController singleton;

	public int GetResult()
	{
		return count;
	}

	public override void _Ready()
	{
		singleton = this;
		GetNode<TextureButton>(upButton).Pressed += goUp;
		GetNode<TextureButton>(downButton).Pressed += goDown;
		refreshLabel();
	}

	void refreshLabel()
	{
		count = Math.Clamp(count, 0, 20);
		GetNode<Label>(label).Text = count.ToString();
	}

	void goUp()
	{
		count++;
		refreshLabel();
	}

	void goDown()
	{
		count--;
		refreshLabel();
	}

	public override void _Process(double delta)
	{
	}
}
