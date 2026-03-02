using UnityEngine;

public class AI_SHOOT : MonoBehaviour
{
	[Header("Detection Settings")]
	public Transform target;

	public float detectionRadius = 10f;

	[Header("Shooting Settings")]
	public GameObject projectilePrefab;

	public Transform shootPoint;

	public float shootForce = 10f;

	public float fireRate = 1f;

	private float nextFireTime;

	private void Update()
	{
		DetectAndShoot();
	}

	private void DetectAndShoot()
	{
		if (!(target == null) && Vector3.Distance(base.transform.position, target.position) <= detectionRadius && Time.time >= nextFireTime)
		{
			Shoot();
			nextFireTime = Time.time + 1f / fireRate;
		}
	}

	private void Shoot()
	{
		if (projectilePrefab != null && shootPoint != null)
		{
			Rigidbody component = Object.Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation).GetComponent<Rigidbody>();
			if (component != null)
			{
				component.velocity = shootPoint.forward * shootForce;
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, detectionRadius);
	}
}
