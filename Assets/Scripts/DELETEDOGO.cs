using UnityEngine;

public class DELETEDOGO : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("Игрок коснулся триггера!");
		}
	}
}
