using UnityEngine;

public class RANDOM : MonoBehaviour
{
	[Range(0f, 100f)]
	public float spawnChance = 25f;

	private void Start()
	{
		if (Random.Range(0f, 100f) > spawnChance)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
