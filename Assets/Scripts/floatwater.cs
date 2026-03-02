using UnityEngine;

public class floatwater : MonoBehaviour
{
	public Rigidbody rigibody;

	public float depthBefore = 1f;

	public float displace = 3f;

	private void FixedUpdate()
	{
		if (base.transform.position.y < 0f)
		{
			Mathf.Clamp01((0f - base.transform.position.y) / depthBefore);
			_ = displace;
			rigibody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displace, 0f), ForceMode.Acceleration);
		}
	}
}
