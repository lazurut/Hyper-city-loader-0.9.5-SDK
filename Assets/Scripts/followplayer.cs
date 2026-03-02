using UnityEngine;

public class followplayer : MonoBehaviour
{
	public Transform player;

	public float followSpeed = 5f;

	public float stopDistance = 2f;

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, player.position) > stopDistance)
		{
			Vector3 normalized = (player.position - base.transform.position).normalized;
			base.transform.position += normalized * followSpeed * Time.deltaTime;
			base.transform.LookAt(player);
		}
	}
}
