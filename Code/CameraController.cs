using Godot;
using System;
using System.Diagnostics;

public partial class CameraController : Node
{
	Node3D parent;
	bool isInMovementArea = false;
	Vector3 goalRotation;

	public override void _Ready()
	{
		parent = GetParent<Node3D>();
		goalRotation = parent.Rotation;
	}

	void MoveCamera(Vector3 axis)
	{
		goalRotation = goalRotation.Rotated(axis, float.Pi / 2.0f);
		isInMovementArea = true;
	}

	float GetYMouse()
	{
		return GetViewport().GetMousePosition().Y / GetViewport().GetVisibleRect().Size.Y;
	}

	float GetXMouse()
	{
		return GetViewport().GetMousePosition().X / GetViewport().GetVisibleRect().Size.X;
	}

	public override void _Process(double delta)
	{
		Debug.WriteLine(goalRotation);
		parent.Rotation = parent.Rotation.Lerp(goalRotation, 0.5f);

		if (!isInMovementArea)
		{
			if (GetXMouse() > 0.9f)
			{
				MoveCamera(new Vector3(1.0f, 0.0f, 0.0f));
			}
			else if (GetXMouse() < 0.1f)
			{
				MoveCamera(new Vector3(-1.0f, 0.0f, 0.0f));
			}
			else if (GetYMouse() < 0.1f)
			{
				MoveCamera(new Vector3(0.0f, -1.0f, 0.0f));
			}
			else if (GetYMouse() > 0.9f)
			{
				MoveCamera(new Vector3(0.0f, 1.0f, 0.0f));
			}
		} else if (GetYMouse() < 0.8f && GetYMouse() > 0.2f && GetXMouse() < 0.8f && GetXMouse() > 0.2f)
		{
			isInMovementArea = false;
		}
	}
}
