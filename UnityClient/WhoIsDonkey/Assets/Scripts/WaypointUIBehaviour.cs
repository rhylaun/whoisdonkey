using UnityEngine;

public class WaypointUIBehaviour : MonoBehaviour
{
	public GameObject Panel;
	public float ActivationDistance = 0.8f;
	public Color Color = Color.green;

	void FixedUpdate()
	{
		if (Panel == null) return;

		var distance = (this.transform.position - Camera.main.transform.position).magnitude;
		if (distance > ActivationDistance && Panel.activeSelf)
			Panel.SetActive(false);

		if (distance <= ActivationDistance && !Panel.activeSelf)
			Panel.SetActive(true);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color;
		Gizmos.DrawWireSphere(transform.position, ActivationDistance);
	}
}
