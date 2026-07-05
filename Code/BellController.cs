using Godot;
using System;
using System.Diagnostics;

enum Fade
{
	NONE,
	OUT,
	IN,
}

public partial class BellController : StaticBody3D
{
	[Export] MeshInstance3D mesh;
	float goalScale = 1.0f;
	Vector3 startingScale;

	[Export] float fadeLength = 0.5f;
	float fadeProgress = 0.0f;
	Fade fade = Fade.NONE;

	[Export] NodePath colorRect;

	public override void _Ready()
	{
		startingScale = Scale;
	}

	public override void _Process(double delta)
	{
		if (fade != Fade.NONE)
		{
			fadeProgress += (float)delta / fadeLength;
			if (fadeProgress > 1.0f)
			{
				fadeProgress -= 1.0f;
				if (fade == Fade.IN)
				{
					fade = Fade.NONE;
					GetNode<ColorRect>(colorRect).MouseFilter = Control.MouseFilterEnum.Ignore;
				}
				else if (fade == Fade.OUT)
				{
					CharacterController.instance.initCharacter();
					fade = Fade.IN;
				}
			}
		}
		float fadeAmount = 0.0f;
		if (fade == Fade.IN) fadeAmount = 1.0f - fadeProgress;
		if (fade == Fade.OUT) fadeAmount = fadeProgress;
		if (fade == Fade.NONE) fadeAmount = 0.0f;
		GetNode<ColorRect>(colorRect).Modulate = new Color(1.0f, 1.0f, 1.0f, fadeAmount);
	}

	public override void _PhysicsProcess(double delta)
	{
		Scale = startingScale.Lerp(startingScale * goalScale, 0.1f);
	}

	public override void _MouseEnter()
	{
		base._MouseEnter();
		goalScale = 1.4f;
	}

	public override void _MouseExit()
	{
		base._MouseExit();
		goalScale = 1.0f;
	}
	
	void changeCharacter()
	{
		fade = Fade.OUT;
		GetNode<ColorRect>(colorRect).MouseFilter = Control.MouseFilterEnum.Stop;
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 eventPosition, Vector3 normal, int shapeIdx)
	{
		base._InputEvent(camera, @event, eventPosition, normal, shapeIdx);

		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton buttonEvent = (InputEventMouseButton) @event;
			if (buttonEvent.ButtonIndex == MouseButton.Left && !buttonEvent.IsReleased())
			{
				changeCharacter();
			}
		}
	}
}
