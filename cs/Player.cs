using Godot;
using System;

public class Player : RigidBody
{
	private float forceMultiplier = 0.1f;
	private float jumpForce = 4f;
	private float maxSpeed = 1.5f;
	private float right = 0.0f;
	private float left = 0.0f;
	private float up = 0.0f; 
	private float down = 0.0f;
	private bool jump = false;
	private Anchor _cameraAnchor;
	private AnimatedSprite3D[] _faces;

	public override void _Ready()
	{
		ContactMonitor = true;
		_cameraAnchor = GetNode<Anchor>("Anchor");
		_faces = new AnimatedSprite3D[] {
			GetNode<AnimatedSprite3D>("Mite"),
			GetNode<AnimatedSprite3D>("Backface")
		};
	}

	public override void _PhysicsProcess(float delta)
	{
		var force = new Vector3(right - left, 0, down - up) * forceMultiplier;
		foreach (var face in _faces)
		{
			// Only animate the faces when there is movement
			// this will need to change when I add multiple animations for the same entity
			face.Playing = Math.Abs(right - left) + Math.Abs(down - up) > 0;
		}

		var globalForce = GlobalTransform.basis.Xform(force);
		if (Math.Abs(LinearVelocity.y) < 0.2)
			ApplyImpulse(Vector3.Zero, globalForce.Rotated(
				new Vector3(0, 1, 0),
				_cameraAnchor.localRotation + 
				/* starting offset for camera */
				(float)(Math.PI / 4)
			));

		if (jump)
		{
			// Don't jump unless your are in a position where you aren't already falling or jumping
			if (Math.Abs(LinearVelocity.y) < 0.1)
			{
				ApplyCentralImpulse(new Vector3(0, jumpForce, 0));
			}
			jump = false;
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		base._IntegrateForces(state);
		Rotation = Vector3.Zero;
		var newVelocity = new Vector3(LinearVelocity);
		// this could be done a bit better
		if (Math.Abs(LinearVelocity.x) > maxSpeed)
			newVelocity.x = maxSpeed * (Math.Abs(LinearVelocity.x) /LinearVelocity.x);
		if (Math.Abs(LinearVelocity.z) > maxSpeed)
			newVelocity.z = maxSpeed * (Math.Abs(LinearVelocity.z) / LinearVelocity.z);
		LinearVelocity = newVelocity;

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// left stick is also mapped to WASD
		if (@event.IsAction("left_stick_right"))
		{
			right = @event.GetActionStrength("left_stick_right");
		}
		if (@event.IsAction("left_stick_left"))
		{
			left = @event.GetActionStrength("left_stick_left");
		}
		if (@event.IsAction("left_stick_down"))
		{
			down = @event.GetActionStrength("left_stick_down");
		}
		if (@event.IsAction("left_stick_up"))
		{
			up = @event.GetActionStrength("left_stick_up");
		}
		if (@event.IsAction("ui_accept"))
		{
			jump = @event.GetActionStrength("ui_accept") == 1;
		}
	}
}
