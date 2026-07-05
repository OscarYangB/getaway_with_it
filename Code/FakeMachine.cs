using Godot;
using System;
using System.Diagnostics;

public partial class FakeMachine : Node3D
{
	[Export] NodePath passport;
	[Export] NodePath passportLabel;
	[Export] float distance = 0.2f;
	[Export] NodePath label;
	bool hasDocument = false;
	[Export] Vector3 releasePosition = new Vector3(0.0f, 0.95f, 2.9f);

	public override void _Ready()
	{
		GetNode<Label>(label).Text = "identity fraud machine";
	}

	void Release()
	{
		Interactable interactable = GetNode<Interactable>(passport);
		interactable.Position = releasePosition;
		interactable.Visible = true;
		CameraController.singleton.EnableMovement();
		hasDocument = false;
		GetNode<Label>(passportLabel).Text = GetNode<Label>(label).Text;
		GetNode<Label>(label).Text = "identity fraud 3000";
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey && hasDocument)
		{
			InputEventKey keyEvent = @event as InputEventKey;
			char key = keyEvent.AsTextKeycode()[0];
			if (!keyEvent.IsReleased())
			{
				if (keyEvent.Keycode == Key.Shift)
				{
					// nothing
				}
				else if (keyEvent.Keycode == Key.Backspace)
				{
					GetNode<Label>(label).Text = GetNode<Label>(label).Text.Remove(GetNode<Label>(label).Text.Length - 1);
				}
				else if (keyEvent.Keycode == Key.Space)
				{
					GetNode<Label>(label).Text += " ";
				}
				else if (keyEvent.Keycode == Key.Enter)
				{
					Release();
				}
				else if (char.IsLetter(key))
				{
					GetNode<Label>(label).Text += Input.IsKeyPressed(Key.Shift) ? key.ToString().ToUpper() : key.ToString().ToLower();
				}
			}
		}
	}

	public override void _Process(double delta)
	{
		Interactable interactable = GetNode<Interactable>(passport);

		if (interactable.Position.DistanceTo(Position) < distance && !hasDocument)
		{
			interactable.Drop();
			interactable.Visible = false;
			CameraController.singleton.DisableMovement();
			GetNode<Label>(label).Text = GetNode<Label>(passportLabel).Text;
			hasDocument = true;
		}
	}
}
