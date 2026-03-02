using UnityEngine;

public class dialoge : MonoBehaviour
{
	public GameObject panel;

	private void Start()
	{
		if (panel != null)
		{
			panel.SetActive(value: false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && panel != null)
		{
			panel.SetActive(value: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && panel != null)
		{
			panel.SetActive(value: false);
		}
	}
}
