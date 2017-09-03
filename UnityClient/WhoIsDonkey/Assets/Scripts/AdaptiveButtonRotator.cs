using UnityEngine;

public class AdaptiveButtonRotator : MonoBehaviour
{
	public float AngleSpeed = 1f;

	void Update()
	{
		this.transform.Rotate(Vector3.forward, AngleSpeed * Time.deltaTime);
	}
}
