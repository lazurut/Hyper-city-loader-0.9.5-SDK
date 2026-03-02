using UnityEngine;

public class CameraTriger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			OpenCamera();
		}
	}

	private void OpenCamera()
	{
	}
}
