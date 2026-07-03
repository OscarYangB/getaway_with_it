using Godot;
using Godot.Collections;
using System;
using System.Diagnostics;

public partial class CameraController : Camera3D
{
	[Export] Array<CameraState> states;
	CameraState currentState;
	bool isInMovementArea = false;
	Vector3 goalRotation;

	public override void _Ready()
	{
		currentState = states[0];
		goalRotation = Rotation;
	}

	void MoveCamera(CameraDirection direction)
	{
		if (currentState.transitions.TryGetValue(direction, out int newState))
		{
			currentState = states[newState];
			goalRotation = currentState.rotation;
			isInMovementArea = true;
		}
	}

	float GetYMouse()
	{
		return GetViewport().GetMousePosition().Y / GetViewport().GetVisibleRect().Size.Y;
	}

	float GetXMouse()
	{
		return GetViewport().GetMousePosition().X / GetViewport().GetVisibleRect().Size.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		Rotation = Rotation.Lerp(goalRotation * (float.Pi / 180.0f), 0.5f);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("up"))
		{
			MoveCamera(CameraDirection.UP);
		} 
		else if (@event.IsActionPressed("down"))
		{
			MoveCamera(CameraDirection.DOWN);
		}
		else if (@event.IsActionPressed("right"))
		{
			MoveCamera(CameraDirection.RIGHT);
		}
		else if (@event.IsActionPressed("left"))
		{
			MoveCamera(CameraDirection.LEFT);
		}
	}

	public override void _Process(double delta)
	{
		if (!isInMovementArea)
		{
			if (GetXMouse() > 0.9f)
			{
				MoveCamera(CameraDirection.RIGHT);
			}
			else if (GetXMouse() < 0.1f)
			{
				MoveCamera(CameraDirection.LEFT);
			}
			else if (GetYMouse() < 0.1f)
			{
				MoveCamera(CameraDirection.UP);
			}
			else if (GetYMouse() > 0.9f)
			{
				MoveCamera(CameraDirection.DOWN);
			}
		} else if (GetYMouse() < 0.8f && GetYMouse() > 0.2f && GetXMouse() < 0.8f && GetXMouse() > 0.2f)
		{
			isInMovementArea = false;
		}
	}
}
