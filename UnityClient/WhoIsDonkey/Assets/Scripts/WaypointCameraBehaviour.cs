using UnityEngine;
using Donkey.Client;
using Donkey.Common;
using System.Collections;

public class WaypointCameraBehaviour : MonoBehaviour
{
	public GameObject Target;
	public float Speed = 0.2f;
	public PlayerState StayUntil;

	private bool _waypointReached = false;

	void FixedUpdate()
	{
		if (GameClientManager.Current.State != StayUntil)
		{
			_waypointReached = false;
			return;
		}

		if (_waypointReached) return;

		var camera = Camera.main;
		var startPos = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
		MoveTowards(camera, Speed);
		_waypointReached = CheckOverrun(startPos, camera.transform.position);
		if (_waypointReached)
			camera.transform.position = transform.position;
		camera.transform.LookAt(Target.transform.position);
	}

	private void MoveTowards(Camera camera, float speed)
	{
		var distanceVector = transform.position - camera.transform.position;
		var directionVector = distanceVector.normalized;

		camera.transform.position += speed * directionVector * Time.deltaTime;
	}

	private bool CheckOverrun(Vector3 toPoint, Vector3 fromPoint)
	{
		var distance = toPoint - fromPoint;
		var currentDistance = this.transform.position - fromPoint;

		return distance.magnitude < currentDistance.magnitude;
	}
	
}
