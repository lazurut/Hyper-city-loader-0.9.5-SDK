using UnityEngine;

public class size : MonoBehaviour
{
	[SerializeField]
	private float sizeIncreasePercentage = 5f;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Transform obj = other.transform;
			Vector3 localScale = obj.localScale * (1f + sizeIncreasePercentage / 100f);
			obj.localScale = localScale;
			Debug.Log("Игрок увеличен на " + sizeIncreasePercentage + "%!");
		}
	}
}
