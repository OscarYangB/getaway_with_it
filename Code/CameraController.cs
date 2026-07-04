using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class CameraController : Camera3D
{
	[Export] Array<CameraState> states;
	[Export] Dictionary<CameraDirection, NodePath> arrows;
	[Export] WorldEnvironment worldEnvironment;
	[Export] float stateTransitionLength = 0.1f;
	[Export] NodePath dialogNode;
	CameraState oldState;
	CameraState currentState;
	bool isInMovementArea = false;
	Vector3 goalRotation;
	float stateTransitionProgress = 0.0f;
	bool disableCameraMovement = false;
	public static CameraController singleton;

	public void DisableMovement()
	{
		disableCameraMovement = true;
		foreach (var entry in arrows)
		{
			GetNode<CanvasItem>(entry.Value).Visible = false;
		}
	}

	public void EnableMovement()
	{
		disableCameraMovement = false;
		foreach (var entry in arrows)
		{
			GetNode<CanvasItem>(entry.Value).Visible = true;
		}
	}

	public override void _Ready()
	{
		singleton = this;
		currentState = states[0];
		oldState = currentState;
		goalRotation = Rotation;
	}

	void MoveCamera(CameraDirection direction)
	{
		if (currentState.transitions.TryGetValue(direction, out int newState))
		{
			oldState = currentState;
			currentState = states[newState];
			goalRotation = currentState.rotation;
			isInMovementArea = true;
			stateTransitionProgress = 0.0f;
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

	Color getArrowColor(CameraState cameraState, CameraDirection cameraDirection)
	{
		return cameraState.transitions.ContainsKey(cameraDirection) ? Color.Color8(255, 255, 255) : Color.Color8(100, 100, 100, 40);
	}

	public override void _Process(double delta)
	{
		if (!isInMovementArea && !disableCameraMovement)
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

		foreach (var cameraDirection in Enum.GetValues(typeof(CameraDirection)).Cast<CameraDirection>()) {
			Color oldTargetColor = getArrowColor(oldState, cameraDirection);
			Color newTargetColor = getArrowColor(currentState, cameraDirection);
			Color color = oldTargetColor.Lerp(newTargetColor, stateTransitionProgress);
			GetNode<CanvasItem>(arrows[cameraDirection]).Modulate = color;
		}

		float farBlur = Mathf.Lerp(oldState.blurFarDistance, currentState.blurFarDistance, stateTransitionProgress);
		((CameraAttributesPractical)worldEnvironment.CameraAttributes).DofBlurFarDistance = farBlur;
		float nearBlur = Mathf.Lerp(oldState.blurNearDistance, currentState.blurNearDistance, stateTransitionProgress);
		((CameraAttributesPractical)worldEnvironment.CameraAttributes).DofBlurNearDistance = nearBlur;

		stateTransitionProgress += (float)delta * 1.0f / stateTransitionLength;
		stateTransitionProgress = Math.Clamp(stateTransitionProgress, 0.0f, 1.0f);

		float oldDialogVisibility = oldState.isDialogVisible ? 1.0f : 0.0f;
		float newDialogVisibility = currentState.isDialogVisible ? 1.0f : 0.0f;
		GetNode<CanvasItem>(dialogNode).Modulate = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(oldDialogVisibility, newDialogVisibility, stateTransitionProgress));
		GetNode<Control>(dialogNode).MouseFilter = newDialogVisibility > 0.0f ? Control.MouseFilterEnum.Pass : Control.MouseFilterEnum.Ignore;

	}
}
