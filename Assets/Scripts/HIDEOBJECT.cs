using UnityEngine;

public class HIDEOBJECT : MonoBehaviour
{
	public string targetTag = "HIDE";

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(targetTag))
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
}
