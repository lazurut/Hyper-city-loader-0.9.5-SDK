using UnityEngine;
using UnityEngine.AI;

public class NPS : MonoBehaviour
{
	public Transform target;

	private NavMeshAgent agent;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		agent.destination = target.position;
	}
}
