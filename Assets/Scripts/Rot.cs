using UnityEngine;

public class Rot : MonoBehaviour
{
	public float rotationSpeed = 100f;

	private void Update()
	{
		base.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
	}
}
