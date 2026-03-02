using UnityEngine;

public class SELVEL8 : MonoBehaviour
{
	public GameObject bulletPrefab;

	public Transform firePoint;

	public float bulletSpeed = 20f;

	public float fireRate = 0.5f;

	public float cooldownTime = 5f;

	public ParticleSystem muzzleFlash;

	public AudioClip shootSound;

	public AudioSource audioSource;

	public Animator animator;

	public string shootAnimationTrigger = "Shoot";

	private float nextFireTime;

	private bool isCooldown;

	private void Update()
	{
		if (Input.GetButton("Fire1") && Time.time >= nextFireTime && !isCooldown)
		{
			Shoot();
			StartCooldown();
		}
	}

	private void Shoot()
	{
		Rigidbody component = Object.Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody>();
		if (component != null)
		{
			component.velocity = firePoint.forward * bulletSpeed;
		}
		if (muzzleFlash != null)
		{
			muzzleFlash.Play();
		}
		if (audioSource != null && shootSound != null)
		{
			audioSource.PlayOneShot(shootSound);
		}
		if (animator != null)
		{
			animator.SetTrigger(shootAnimationTrigger);
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
