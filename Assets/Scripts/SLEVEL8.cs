using UnityEngine;

public class SLEVEL8 : MonoBehaviour
{
	public Camera camera1;

	public Camera camera2;

	private void Start()
	{
		camera1.enabled = true;
		camera2.enabled = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchToCamera1();
		}
		if (Input.GetKeyDown(KeyCode.RightAlt))
		{
			SwitchToCamera2();
		}
	}

	private void SwitchToCamera1()
	{
		camera1.enabled = true;
		camera2.enabled = false;
	}

	private void SwitchToCamera2()
	{
		camera1.enabled = false;
		camera2.enabled = true;
	}
}
