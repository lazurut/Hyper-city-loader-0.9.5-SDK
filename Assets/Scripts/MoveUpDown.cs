using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
	public float speed = 2f;

	public float height = 1f;

	private Vector3 startPos;

	private void Start()
	{
		startPos = base.transform.position;
	}

	private void Update()
	{
		float y = startPos.y + Mathf.Sin(Time.time * speed) * height;
		base.transform.position = new Vector3(startPos.x, y, startPos.z);
	}
}
