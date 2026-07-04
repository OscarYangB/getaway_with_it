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
	[Export] public float blurFarDistance = 8000.0f;
	[Export] public float blurNearDistance = 0.1f;
	[Export] public bool isDialogVisible = false;

	public CameraState()
	{
		rotation = Vector3.Zero;
		transitions = new();
	}
};
