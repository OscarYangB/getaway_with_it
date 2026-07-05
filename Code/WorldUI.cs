using Godot;
using System;
using System.Diagnostics;

public partial class WorldUI : StaticBody3D
{
	[Export] CollisionShape3D collider;
	[Export] SubViewport subviewport;
	[Export] Vector2 scalingVector = new Vector2(0.96f, 0.54f);

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
        Debug.WriteLine(mouse2D);
        mouse2D /= scalingVector;
		mouse2D += Vector2.One;
		mouse2D /= 2.0f;
        mouse2D *= subviewport.Size;

        if (@event is InputEventMouseButton)
		{
			InputEventMouseButton newEvent = (InputEventMouseButton)@event;
			newEvent.Position = mouse2D;
			subviewport.PushInput(newEvent);
			//Debug.WriteLine(mouse2D);
        }
        else if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion newEvent = (InputEventMouseMotion)@event;
			newEvent.Position = mouse2D;
			subviewport.PushInput(newEvent);
		}
	}

	public override void _Process(double delta)
	{
	}
}
