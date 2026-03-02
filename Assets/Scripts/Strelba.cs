using UnityEngine;

public class Strelba : MonoBehaviour
{
	public float moveSpeed = 5f;

	private Camera mainCamera;

	private void Start()
	{
		mainCamera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			Vector3 forward = mainCamera.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			base.transform.position += forward * moveSpeed * Time.deltaTime;
		}
	}
}
