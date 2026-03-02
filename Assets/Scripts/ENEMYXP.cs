using UnityEngine;

public class ENEMYXP : MonoBehaviour
{
	[SerializeField]
	private GameObject targetPrefab;

	private int hitCount;

	private const int maxHits = 10;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(targetPrefab.tag))
		{
			hitCount++;
			Debug.Log($"Попадание: {hitCount}/10");
			if (hitCount >= 10)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
