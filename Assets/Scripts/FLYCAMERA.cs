using UnityEngine;

public class FLYCAMERA : MonoBehaviour
{
	public float speed = 10f;

	public float sensitivity = 2f;

	private float rotationX;

	private float rotationY;

	private void Update()
	{
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		Vector3 direction = new Vector3(axis, 0f, axis2);
		direction = base.transform.TransformDirection(direction);
		base.transform.position += direction * speed * Time.deltaTime;
		rotationX += Input.GetAxis("Mouse X") * sensitivity;
		rotationY += Input.GetAxis("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp(rotationY, -90f, 90f);
		base.transform.localRotation = Quaternion.Euler(0f - rotationY, rotationX, 0f);
	}
}
