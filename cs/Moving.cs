using Godot;
using System;

public class Moving : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Vector3 endPoint;
	private float speed = 0.01f;
	private bool isGoing = true;
	private Vector3 startPoint;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		endPoint = new Vector3(GetNode<Spatial>("End Point").GlobalTranslation.x, GetNode<Spatial>("End Point").GlobalTranslation.y, GetNode<Spatial>("End Point").GlobalTranslation.z);
		startPoint = new Vector3(Translation.x, Translation.y, Translation.z);
	}

	private static double CalculateDistance(Vector3 a, Vector3 b) {
		return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
	}

	private void TravelTowardsPoint(Vector3 point, Action onDestinationReached)
	{
		var distance = CalculateDistance(point, Translation);
		if (distance < 0.25)
		{
			onDestinationReached();
		}
		else
		{
			var forceVector = Vector3.Zero;
			if (point.z > Translation.z)
			{
				forceVector += new Vector3(0, 0, speed);
			}
			if (point.z < Translation.z)
			{
				forceVector -= new Vector3(0, 0, speed);
			}
			if (point.y > Translation.y)
			{
				forceVector += new Vector3(0, speed, 0);
			}
			if (point.y < Translation.y)
			{
				forceVector -= new Vector3(0, speed, 0);
			}
			if (point.x > Translation.x)
			{
				forceVector += new Vector3(speed, 0, 0);
			}
			if (point.x < Translation.x)
			{
				forceVector -= new Vector3(speed, 0, 0);
			}
			Translation += forceVector;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		// Gosh, isn't it just . . . beautiful?
		if (isGoing) {
			TravelTowardsPoint(endPoint, () =>
			{
				isGoing = false;
			});
		} else {
			TravelTowardsPoint(startPoint, () =>
			{
				isGoing = true;
			});
		}
	}
}
