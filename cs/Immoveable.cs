using Godot;

/// <summary>
/// A rigid body that does not budge; although, it does rotate.
/// </summary>
public class Immoveable : RigidBody
{
	private Vector3 _initialPosition;
	public override void _Ready()
	{
		_initialPosition = Translation;
	}

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		base._IntegrateForces(state);
		Translation = _initialPosition;
		// Discard rotation around any other access than Y
		Rotation = new Vector3(0, 0, 0);
    }
}
