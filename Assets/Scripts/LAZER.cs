using System.Collections;
using UnityEngine;

public class LAZER : MonoBehaviour
{
	public GameObject objectToSpawn;

	public Transform spawnPoint;

	private bool canSpawn = true;

	private bool timerRunning;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && canSpawn && !timerRunning)
		{
			StartCoroutine(SpawnObjectWithCooldown());
		}
	}

	private IEnumerator SpawnObjectWithCooldown()
	{
		Object.Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
		timerRunning = true;
		yield return new WaitForSeconds(5f);
		canSpawn = false;
		timerRunning = false;
		yield return new WaitForSeconds(10f);
		canSpawn = true;
	}
}
