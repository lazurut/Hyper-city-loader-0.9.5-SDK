using UnityEngine;

public class PLAYMODE : MonoBehaviour
{
	public GameObject targetObject;

	public Camera mainCamera;

	public Camera targetCamera;

	private bool isObjectActive;

	private void Start()
	{
		targetObject.SetActive(value: false);
		mainCamera.enabled = true;
		targetCamera.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			isObjectActive = !isObjectActive;
			targetObject.SetActive(isObjectActive);
			if (isObjectActive)
			{
				mainCamera.enabled = false;
				targetCamera.enabled = true;
			}
			else
			{
				mainCamera.enabled = true;
				targetCamera.enabled = false;
			}
		}
	}
}
