using UnityEngine;

public class TELEPORT23 : MonoBehaviour
{
	[SerializeField]
	private Transform targetLocation;

	private void OnCollisionEnter(Collision collision)
	{
		if (targetLocation != null)
		{
			collision.transform.position = targetLocation.position;
			collision.transform.rotation = targetLocation.rotation;
		}
		else
		{
			Debug.LogWarning("Target location is not set in the inspector!");
		}
	}
}
