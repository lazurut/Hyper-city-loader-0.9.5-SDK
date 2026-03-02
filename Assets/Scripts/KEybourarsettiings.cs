using UnityEngine;

public class KEybourarsettiings : MonoBehaviour
{
	private bool useArrowKeys = true;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			useArrowKeys = !useArrowKeys;
			Debug.Log("Переключено управление");
		}
		float x = (useArrowKeys ? Input.GetAxis("Horizontal") : Input.GetAxis("Horizontal_Alt"));
		float z = (useArrowKeys ? Input.GetAxis("Vertical") : Input.GetAxis("Vertical_Alt"));
		base.transform.Translate(new Vector3(x, 0f, z) * Time.deltaTime * 5f);
	}
}
