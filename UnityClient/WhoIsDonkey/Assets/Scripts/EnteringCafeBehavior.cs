using UnityEngine;

public class EnteringCafeBehavior : MonoBehaviour
{

	public float RotationSpeed = 10f;

	public GameObject Point1;
	public float Speed1 = 0.5f;

	public GameObject Point2;
	public float Speed2 = 0.5f;

	private bool _point1Reached = false;
	private bool _point2Reached = false;
	private bool _rotated = false;
	private Vector3 _startPosition;

	void Start()
	{
		_startPosition = this.transform.position;
	}

	void FixedUpdate()
	{
		if (Point1 == null) return;
		if (!_point1Reached)
		{
			MoveTowards(Point1.transform.position, Speed1);
			_point1Reached = CheckOverrun(Point1.transform.position, _startPosition);
			if (_point1Reached)
				this.transform.position = Point1.transform.position;
			return;
		}

		if (Point2 == null) return;
		if (!_rotated)
		{
			var toDir = Point2.transform.position - Point1.transform.position;			
			_rotated = RotateTowards(toDir, RotationSpeed);
			return;
		}

		if (!_point2Reached)
		{
			MoveTowards(Point2.transform.position, Speed2);
			_point2Reached = CheckOverrun(Point2.transform.position, Point1.transform.position);
			if (_point2Reached)
				this.transform.position = Point2.transform.position;
			return;
		}
	}

	private void MoveTowards(Vector3 point, float speed)
	{
		var distanceVector = point - this.transform.position;
		var directionVector = distanceVector.normalized;

		this.transform.LookAt(point);
		this.transform.position += speed * directionVector * Time.deltaTime;
	}

	private bool CheckOverrun(Vector3 toPoint, Vector3 fromPoint)
	{
		var distance = toPoint - fromPoint;
		var currentDistance = this.transform.position - fromPoint;

		return distance.magnitude < currentDistance.magnitude;
	}

	private bool RotateTowards(Vector3 toDir, float speed)
	{
		var fullRotation = Quaternion.LookRotation(toDir);
		var result = Quaternion.RotateTowards(this.transform.rotation, fullRotation, speed * Time.deltaTime);
		if (result == this.transform.rotation) return true;

		this.transform.rotation = result;
		return false;
	}
}
