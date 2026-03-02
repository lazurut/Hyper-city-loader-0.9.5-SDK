using UnityEngine;

public class CAMERAOFF : MonoBehaviour
{
	public float moveSpeed = 10f;

	private bool isCameraMovementEnabled = true;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			isCameraMovementEnabled = !isCameraMovementEnabled;
		}
		if (isCameraMovementEnabled)
		{
			MoveCamera();
		}
	}

	private void MoveCamera()
	{
		float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
		base.transform.Translate(x, 0f, z);
	}
}
