using Godot;
using System;
using System.Diagnostics;

public partial class Interactable : StaticBody3D
{
	bool isLifted = false;
	[Export] MeshInstance3D mesh;
	float goalOutline = 0.0f;
	[Export] float droppedHeight = 0.95f;
	[Export] float liftedHeight = 1.0f;
	Vector2 startingMouse;
	Vector3 startingPosition;

	Vector2 GetMouse()
	{
		return new Vector2(GetViewport().GetMousePosition().X / GetViewport().GetVisibleRect().Size.X,
						   GetViewport().GetMousePosition().Y / GetViewport().GetVisibleRect().Size.Y);
	}

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (!Input.IsMouseButtonPressed(MouseButton.Left) && isLifted)
		{
			isLifted = false;
			goalOutline = 0.0f;
			Input.MouseMode = Input.MouseModeEnum.Visible;
			CameraController.singleton.EnableMovement();
		}

		if (isLifted)
		{
			Vector2 mouseDelta = GetMouse() - startingMouse;
			Position = new Vector3(startingPosition.X + mouseDelta.X, Position.Y, startingPosition.Z + mouseDelta.Y);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness = Mathf.Lerp(((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness, goalOutline, 0.5f);

		Position = new Vector3(Position.X, Mathf.Lerp(Position.Y, isLifted ? liftedHeight : droppedHeight, 0.5f), Position.Z);
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
				startingMouse = GetMouse();
				startingPosition = Position;
				Input.MouseMode = Input.MouseModeEnum.Hidden;
				CameraController.singleton.DisableMovement();
			}
		}
	}
}
