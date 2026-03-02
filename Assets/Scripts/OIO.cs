using UnityEngine;

public class OIO : MonoBehaviour
{
	public Transform player;

	public float followSpeed = 2f;

	private bool isFollowing;

	private void Update()
	{
		if (isFollowing)
		{
			FollowPlayer();
		}
	}

	private void FollowPlayer()
	{
		base.transform.position = Vector3.Lerp(base.transform.position, player.position, followSpeed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject == player.gameObject)
		{
			isFollowing = true;
		}
	}
}
