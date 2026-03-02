using UnityEngine;

public class RotateEarth : MonoBehaviour
{
	[SerializeField]
	public float speed;

	private void Update()
	{
		base.transform.Rotate(0f, speed * Time.deltaTime, 0f, Space.Self);
	}
}
