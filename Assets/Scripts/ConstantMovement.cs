using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
	public float moveSpeed = 5f;

	private void Update()
	{
		base.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}
}
