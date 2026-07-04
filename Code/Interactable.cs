using Godot;
using System;
using System.Diagnostics;

public partial class Interactable : StaticBody3D
{
	bool isLifted = false;
	[Export] MeshInstance3D mesh;
	float goalOutline = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Input.IsMouseButtonPressed(MouseButton.Left) && isLifted)
		{
			isLifted = false;
			goalOutline = 0.0f;
		}

		if (isLifted)
		{
			// drag around
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness = Mathf.Lerp(((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness, goalOutline, 0.5f);
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton mouseEvent = (InputEventMouseButton)@event;
			if (!mouseEvent.IsReleased() && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				isLifted = true;
				goalOutline = 0.05f;
			}
		}
	}
}
