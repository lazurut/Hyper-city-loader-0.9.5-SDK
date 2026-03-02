using System.Collections;
using UnityEngine;

public class SPAWNER : MonoBehaviour
{
	public GameObject enemyPrefab;

	public Transform spawnPoint;

	public float initialSpawnDelay = 2f;

	public float spawnRateIncreaseInterval = 60f;

	public float spawnRateMultiplier = 0.9f;

	private float currentSpawnDelay;

	private void Start()
	{
		currentSpawnDelay = initialSpawnDelay;
		StartCoroutine(SpawnEnemies());
		StartCoroutine(IncreaseSpawnRateOverTime());
	}

	private IEnumerator SpawnEnemies()
	{
		while (true)
		{
			yield return new WaitForSeconds(currentSpawnDelay);
			SpawnEnemy();
		}
	}

	private void SpawnEnemy()
	{
		Object.Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
	}

	private IEnumerator IncreaseSpawnRateOverTime()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnRateIncreaseInterval);
			currentSpawnDelay *= spawnRateMultiplier;
			currentSpawnDelay = Mathf.Max(currentSpawnDelay, 0.1f);
			Debug.Log("Увеличена скорость спауна. Текущая задержка: " + currentSpawnDelay);
		}
	}
}
