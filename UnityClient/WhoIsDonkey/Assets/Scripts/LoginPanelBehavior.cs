using UnityEngine;
using System.Collections;

public class LoginPanelBehavior : MonoBehaviour
{
	public GameObject DistantObject;
	public GameObject ActivableObject;
	public float ActivationDistance;

	private bool isActive = false;

	void Update()
	{
		if (isActive)
			return;

		if(Input.anyKeyDown)
		{
			Activate();
			return;
		}

		var subVector = DistantObject.transform.position - this.transform.position;
		if (subVector.magnitude - ActivationDistance < 0.001f)
			Activate();
	}

	void Activate()
	{
		ActivableObject.SetActive(true);
		isActive = true;
	}
}
