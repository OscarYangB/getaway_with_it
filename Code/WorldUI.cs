using Godot;
using System;
using System.Diagnostics;

public partial class WorldUI : StaticBody3D
{
	[Export] CollisionShape3D collider;
	[Export] SubViewport subviewport;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shapeIdx)
	{
		base._InputEvent(camera, @event, eventPosition, normal, shapeIdx);
		Vector3 mouse3D = collider.GlobalTransform.AffineInverse() * eventPosition;
		Vector2 mouse2D = new Vector2(mouse3D.X, mouse3D.Y);
		mouse2D *= new Vector2(1.0f, -1.0f);
		mouse2D += Vector2.One / 2;
		mouse2D *= subviewport.Size;

		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton newEvent = (InputEventMouseButton)@event;
			newEvent.Position = mouse2D;
			subviewport.PushInput(newEvent);
		}
		else if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion newEvent = (InputEventMouseMotion)@event;
			newEvent.Position = mouse2D;
			subviewport.PushInput(newEvent);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
