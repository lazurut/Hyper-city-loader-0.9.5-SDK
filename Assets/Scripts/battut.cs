using UnityEngine;

public class battut : MonoBehaviour
{
	[SerializeField]
	private float bounceForce = 10f;

	[SerializeField]
	private float upwardsModifier = 1.5f;

	private void OnCollisionEnter(Collision collision)
	{
		Rigidbody component = collision.collider.GetComponent<Rigidbody>();
		if (component != null)
		{
			Vector3 vector = Vector3.up * upwardsModifier + base.transform.up;
			component.velocity = Vector3.zero;
			component.AddForce(vector.normalized * bounceForce, ForceMode.Impulse);
		}
	}
}
