using UnityEngine;

public class STRELBA1 : MonoBehaviour
{
	public GameObject bulletPrefab;

	public Transform firePoint;

	public float bulletSpeed = 20f;

	public float fireRate = 0.5f;

	private float nextFireTime;

	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
		{
			Shoot();
			nextFireTime = Time.time + fireRate;
		}
	}

	private void Shoot()
	{
		Rigidbody component = Object.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
		if (component != null)
		{
			component.velocity = firePoint.forward * bulletSpeed;
		}
	}
}
