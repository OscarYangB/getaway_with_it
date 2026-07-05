using Godot;
using System;
using System.Diagnostics;

public partial class Interactable : StaticBody3D
{
	bool isLifted = false;
	//[Export] MeshInstance3D mesh;
	float goalOutline = 0.0f;
	[Export] float droppedHeight = 0.95f;
	[Export] float liftedHeight = 1.0f;
	Vector2 startingMouse;
	Vector3 startingPosition;

	[Export] NodePath coverLeft;
	[Export] NodePath pageLeft;
	[Export] NodePath pageRight;
	[Export] NodePath coverRight;

	[Export] Vector3 coverLeftClosed = new Vector3(0.0f, 0.0f, 197.6f);
	[Export] Vector3 pageLeftClosed = new Vector3(0.0f, 0.0f, 192.0f);
	[Export] Vector3 coverRightClosed = new Vector3(0.0f, 0.0f, 0.0f);
	[Export] Vector3 pageRightClosed = new Vector3(0.0f, 0.0f, 0.0f);

	[Export] Vector3 coverLeftOpen = new Vector3(0.0f, 0.0f, -14.4f);
	[Export] Vector3 pageLeftOpen = new Vector3(0.0f, 0.0f, -19.0f);
	[Export] Vector3 coverRightOpen = new Vector3(0.0f, 0.0f, -15.0f);
	[Export] Vector3 pageRightOpen = new Vector3(0.0f, 0.0f, -10.0f);

	bool isOpen = false;

	Vector2 GetMouse()
	{
		return new Vector2(GetViewport().GetMousePosition().X / GetViewport().GetVisibleRect().Size.X,
						   GetViewport().GetMousePosition().Y / GetViewport().GetVisibleRect().Size.Y);
	}

	public override void _Ready()
	{
	}

	public void Drop()
	{
		isLifted = false;
		goalOutline = 0.0f;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		CameraController.singleton.EnableMovement();
		isOpen = false;
	}

	public override void _Process(double delta)
	{
		if (!Input.IsMouseButtonPressed(MouseButton.Left) && isLifted)
		{
			Drop();
		}

		if (isLifted)
		{
			Vector2 mouseDelta = GetMouse() - startingMouse;
			Position = new Vector3(startingPosition.X + mouseDelta.X, Position.Y, startingPosition.Z + mouseDelta.Y);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		//((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness = Mathf.Lerp(((StandardMaterial3D)mesh.GetSurfaceOverrideMaterial(0)).StencilOutlineThickness, goalOutline, 0.5f);

		Position = new Vector3(Position.X, Mathf.Lerp(Position.Y, isLifted ? liftedHeight : droppedHeight, 0.5f), Position.Z);

		GetNode<MeshInstance3D>(coverLeft).Rotation =  new Vector3(0.0f, 0.0f, Mathf.LerpAngle(GetNode<MeshInstance3D>(coverLeft).Rotation.Z, Mathf.DegToRad(isOpen ? coverLeftOpen.Z : coverLeftClosed.Z), 0.2f));
		GetNode<MeshInstance3D>(coverRight).Rotation = new Vector3(0.0f, 0.0f, Mathf.LerpAngle(GetNode<MeshInstance3D>(coverRight).Rotation.Z, Mathf.DegToRad(isOpen ? coverRightOpen.Z : coverRightClosed.Z), 0.2f));
		GetNode<MeshInstance3D>(pageLeft).Rotation = new Vector3(0.0f, 0.0f, Mathf.LerpAngle(GetNode<MeshInstance3D>(pageLeft).Rotation.Z, Mathf.DegToRad(isOpen ? pageLeftOpen.Z : pageLeftClosed.Z), 0.2f));
		GetNode<MeshInstance3D>(pageRight).Rotation = new Vector3(0.0f, 0.0f, Mathf.LerpAngle(GetNode<MeshInstance3D>(pageRight).Rotation.Z, Mathf.DegToRad(isOpen ? pageRightOpen.Z : pageRightClosed.Z), 0.2f));
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
				isOpen = true;
			}
		}
	}
}
