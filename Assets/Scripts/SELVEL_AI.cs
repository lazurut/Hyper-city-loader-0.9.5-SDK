using UnityEngine;
using UnityEngine.AI;

public class SELVEL_AI : MonoBehaviour
{
	public float wanderRadius = 10f;

	public float wanderDelay = 3f;

	private NavMeshAgent agent;

	private float timer;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		if (agent == null)
		{
			Debug.LogError("NavMeshAgent отсутствует на объекте " + base.gameObject.name);
			base.enabled = false;
		}
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer >= wanderDelay)
		{
			Vector3 randomPoint = GetRandomPoint(base.transform.position, wanderRadius);
			agent.SetDestination(randomPoint);
			timer = 0f;
		}
	}

	private Vector3 GetRandomPoint(Vector3 origin, float radius)
	{
		Vector2 vector = Random.insideUnitCircle * radius;
		if (NavMesh.SamplePosition(origin + new Vector3(vector.x, 0f, vector.y), out var hit, radius, -1))
		{
			return hit.position;
		}
		return origin;
	}
}
