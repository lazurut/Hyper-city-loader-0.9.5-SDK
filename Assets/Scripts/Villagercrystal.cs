using System.Collections;
using UnityEngine;

public class Villagercrystal : MonoBehaviour
{
	public GameObject objectToSpawn;

	public Transform spawnPoint;

	public GameObject bulletPrefab;

	public Transform firePoint;

	public float bulletSpeed = 20f;

	public float fireRate = 0.5f;

	public float cooldownTime = 5f;

	public ParticleSystem muzzleFlash;

	public AudioClip shootSound;

	public AudioSource audioSource;

	public Transform target;

	private float nextFireTime;

	private bool isCooldown;

	private bool canSpawn = true;

	private bool timerRunning;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && canSpawn && !timerRunning)
		{
			StartCoroutine(SpawnObjectWithCooldown());
		}
		if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !isCooldown)
		{
			Shoot();
			StartCooldown();
		}
	}

	private IEnumerator SpawnObjectWithCooldown()
	{
		Object.Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
		timerRunning = true;
		yield return new WaitForSeconds(5f);
		canSpawn = false;
		timerRunning = false;
		yield return new WaitForSeconds(10f);
		canSpawn = true;
	}

	private void Shoot()
	{
		Rigidbody component = Object.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
		if (component != null)
		{
			if (target != null)
			{
				Vector3 normalized = (target.position - firePoint.position).normalized;
				component.velocity = normalized * bulletSpeed;
			}
			else
			{
				component.velocity = firePoint.forward * bulletSpeed;
			}
		}
		if (muzzleFlash != null)
		{
			muzzleFlash.Play();
		}
		if (audioSource != null && shootSound != null)
		{
			audioSource.PlayOneShot(shootSound);
		}
		nextFireTime = Time.time + fireRate;
	}

	private void StartCooldown()
	{
		isCooldown = true;
		Invoke("EndCooldown", cooldownTime);
	}

	private void EndCooldown()
	{
		isCooldown = false;
	}
}
