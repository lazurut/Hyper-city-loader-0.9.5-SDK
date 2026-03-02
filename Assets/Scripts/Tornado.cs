using System.Collections;
using UnityEngine;

public class Tornado : MonoBehaviour
{
	public Transform suctionPoint;

	public float suctionForce = 10f;

	public float holdDuration = 2f;

	public float throwForce = 15f;

	public Vector3 throwDirection = Vector3.up;

	private bool isPlayerInside;

	private GameObject player;

	private Rigidbody playerRb;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !isPlayerInside)
		{
			player = other.gameObject;
			playerRb = player.GetComponent<Rigidbody>();
			if (playerRb != null)
			{
				StartCoroutine(SuctionSequence());
			}
		}
	}

	private IEnumerator SuctionSequence()
	{
		isPlayerInside = true;
		while (Vector3.Distance(player.transform.position, suctionPoint.position) > 0.1f)
		{
			Vector3 normalized = (suctionPoint.position - player.transform.position).normalized;
			playerRb.AddForce(normalized * suctionForce, ForceMode.Acceleration);
			yield return null;
		}
		playerRb.velocity = Vector3.zero;
		yield return new WaitForSeconds(holdDuration);
		playerRb.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
		isPlayerInside = false;
	}
}
