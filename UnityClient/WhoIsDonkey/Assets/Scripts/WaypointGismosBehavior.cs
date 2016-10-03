using UnityEngine;
using System.Collections;

public class WaypointGismosBehavior : MonoBehaviour
{
	public float Radius = 0.3f;
	public Color Color = Color.yellow;

	void OnDrawGizmos()
	{
		Gizmos.color = Color;
		Gizmos.DrawSphere(transform.position, Radius);
	}
}
