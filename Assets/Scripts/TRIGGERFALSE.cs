using UnityEngine;

public class TRIGGERFALSE : MonoBehaviour
{
	public GameObject targetObject;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == targetObject)
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
}
