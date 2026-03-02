using UnityEngine;
using UnityEngine.AI;

public class AGENT : MonoBehaviour
{
	private NavMeshAgent agent;

	public GameObject player;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		agent.destination = player.transform.position;
	}
}
