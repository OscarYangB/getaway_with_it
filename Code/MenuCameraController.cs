using Godot;
using System;

public partial class MenuCameraController : Camera3D
{
	Vector3 startingRotation;
	[Export] float intensity = 0.02f;

	Vector2 GetMouse()
	{
		return new Vector2(GetViewport().GetMousePosition().X / GetViewport().GetVisibleRect().Size.X,
						   GetViewport().GetMousePosition().Y / GetViewport().GetVisibleRect().Size.Y);
	}

	public override void _Ready()
	{
		startingRotation = Rotation;
	}

	public override void _Process(double delta)
	{
		Vector2 mouse = GetMouse();
		mouse -= new Vector2(0.5f, 0.5f);
		Rotation = startingRotation + new Vector3(-mouse.Y, -mouse.X , startingRotation.Z) * intensity;
	}
}
