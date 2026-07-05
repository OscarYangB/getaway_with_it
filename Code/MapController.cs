using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

public partial class MapController : Node
{
	[Export] Texture2D offButtonTexture;
	[Export] Texture2D onButtonTexture;
	public static MapController singleton;
	Location selected = null;

	public override void _Ready()
	{
		singleton = this;
		foreach (Node child in GetChildren())
		{
			if (child is Location)
			{
				Location button = (Location)child;
				button.Pressed += () => ButtonPressed(button.Name);
			}
		}
	}

	void ButtonPressed(string name)
	{
		foreach (Node child in GetChildren())
		{
			if (child is Location)
			{
				Location button = (Location)child;
				bool isSelected = button.Name == name;
				button.TextureNormal = isSelected ? onButtonTexture : offButtonTexture;
				if (isSelected)
				{
					selected = button;
				}
			}
		}
	}

	public List<string> GetSelection()
	{
		if (selected == null)
		{
			return new List<string>();
		}

		return selected.tags.ToList();
	}
}
