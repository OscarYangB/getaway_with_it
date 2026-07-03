using Godot;
using Godot.Collections;

public enum CameraDirection
{
	LEFT,
	RIGHT,
	UP,
	DOWN
};

[GlobalClass]
public partial class CameraState : Resource
{
	[Export] public Vector3 rotation;
	[Export] public Dictionary<CameraDirection, int> transitions;
	[Export] public float focusDistance;

	public CameraState()
	{
		rotation = Vector3.Zero;
		transitions = new();
	}
};
