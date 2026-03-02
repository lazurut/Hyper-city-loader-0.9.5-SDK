using UnityEngine;

public class Cameracollision : MonoBehaviour
{
	public Transform target;

	public float minDistance = 1f;

	public float maxDistance = 5f;

	public float smoothSpeed = 10f;

	public LayerMask collisionMask;

	private Vector3 offset;

	private void Start()
	{
		offset = base.transform.position - target.position;
	}

	private void LateUpdate()
	{
		Vector3 b = target.position + offset.normalized * maxDistance;
		if (Physics.Raycast(target.position, offset.normalized, out var hitInfo, maxDistance, collisionMask))
		{
			float num = Mathf.Clamp(hitInfo.distance, minDistance, maxDistance);
			b = target.position + offset.normalized * num;
		}
		base.transform.position = Vector3.Lerp(base.transform.position, b, smoothSpeed * Time.deltaTime);
		base.transform.LookAt(target);
	}
}
