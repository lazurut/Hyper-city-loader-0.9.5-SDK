using UnityEngine;

public class ClickAndLift : MonoBehaviour
{
	public GameObject targetObject;

	public float liftAmount = 5f;

	public AudioSource liftSound;

	private Vector3 originalPosition;

	private void Start()
	{
		if (targetObject != null)
		{
			originalPosition = targetObject.transform.position;
		}
	}

	private void OnMouseDown()
	{
		if (targetObject != null)
		{
			targetObject.transform.position = new Vector3(targetObject.transform.position.x, originalPosition.y + liftAmount, targetObject.transform.position.z);
			if (liftSound != null)
			{
				liftSound.Play();
			}
		}
	}
}
