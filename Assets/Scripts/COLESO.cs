using UnityEngine;

public class COLESO : MonoBehaviour
{
	public Transform player;

	public float rotationSpeed = 100f;

	private float lastYPosition;

	private void Start()
	{
		if (player == null)
		{
			Debug.LogError("Player Transform is not assigned!");
			base.enabled = false;
		}
		else
		{
			lastYPosition = player.position.y;
		}
	}

	private void Update()
	{
		float y = player.position.y;
		if (y != lastYPosition)
		{
			float num = ((y > lastYPosition) ? 1f : (-1f));
			base.transform.Rotate(Vector3.up, num * rotationSpeed * Time.deltaTime);
			lastYPosition = y;
		}
	}
}
