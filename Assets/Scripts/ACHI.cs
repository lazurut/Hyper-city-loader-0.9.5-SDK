using UnityEngine;

public class ACHI : MonoBehaviour
{
	public GameObject panel;

	private bool panelShown;

	private void Start()
	{
		if (panel != null)
		{
			panel.SetActive(value: false);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && panel != null && !panelShown)
		{
			panel.SetActive(value: true);
			panelShown = true;
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
