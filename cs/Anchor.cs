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
    private CanvasLayer _touchControls;
    private bool useTouch
	{
		get
		{
			return _touchControls.Visible;
		}
		set
		{
			_touchControls.Visible = value;
		}
	}

	public override void _Ready()
	{
		_touchControls = GetNode<CanvasLayer>("TouchControls");
	}

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
		if (@event is InputEventScreenTouch && !useTouch)
		{
			useTouch = true;
		}
		if (Input.IsActionPressed("camera_controls"))
		{
			if (@event is InputEventScreenDrag inputEventScreenDrag)
			{
				localRotation += inputEventScreenDrag.Relative.x * rotationSpeedMultiplier;
			}
		}
	}
}
