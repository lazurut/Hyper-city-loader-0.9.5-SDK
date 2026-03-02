using UnityEngine;

public class particle : MonoBehaviour
{
	public ParticleSystem particleSystem;

	private bool canPlay;

	private void Start()
	{
		canPlay = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R) && particleSystem != null && canPlay)
		{
			particleSystem.Play();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			canPlay = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ground"))
		{
			canPlay = false;
		}
	}
}
