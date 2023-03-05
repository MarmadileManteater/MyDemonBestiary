using Godot;
using System;

/// <summary>
/// the anchor that the camera is attached to;
/// it rotates while the camera stays put
/// </summary>
public class Anchor : Spatial
{
	private float rotationSpeedMultiplier = 0.01f;
	public float localRotation = -1.5f;

	public override void _Process(float delta)
	{
		localRotation += (Input.GetActionStrength("right_stick_right") - Input.GetActionStrength("right_stick_left")) * rotationSpeedMultiplier;
		Rotation = new Vector3(0f, localRotation, 0f);
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsMouseButtonPressed(2))
		{
			if (@event is InputEventMouseMotion eventMouseMotion)
			{
				localRotation += eventMouseMotion.Relative.x * rotationSpeedMultiplier;
			}
		}
		if (@event is InputEventScreenDrag inputEventScreenDrag)
		{
			localRotation += inputEventScreenDrag.Relative.x * rotationSpeedMultiplier;
		}
		if (@event.IsActionPressed("ui_left"))
		{
			
			GD.Print("left");
		}
	}
}
