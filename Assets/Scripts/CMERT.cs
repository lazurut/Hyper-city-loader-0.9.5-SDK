using UnityEngine;

public class CMERT : MonoBehaviour
{
	private Vector3 initialPosition;

	private void Start()
	{
		initialPosition = base.transform.position;
	}

	private void OnCollisionEnter(Collision collision)
	{
		base.transform.position = initialPosition;
	}
}
