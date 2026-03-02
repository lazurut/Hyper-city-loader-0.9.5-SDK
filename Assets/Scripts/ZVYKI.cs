using UnityEngine;

public class ZVYKI : MonoBehaviour
{
	public AudioSource movementSound;

	public float movementThreshold = 0.1f;

	private CharacterController controller;

	private Vector3 lastPosition;

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		lastPosition = base.transform.position;
		if (movementSound == null)
		{
			Debug.LogError("AudioSource is not assigned!");
		}
	}

	private void Update()
	{
		Vector3 vector = base.transform.position - lastPosition;
		if (vector.magnitude > movementThreshold && !movementSound.isPlaying)
		{
			movementSound.Play();
		}
		else if (vector.magnitude <= movementThreshold && movementSound.isPlaying)
		{
			movementSound.Stop();
		}
		lastPosition = base.transform.position;
	}
}
