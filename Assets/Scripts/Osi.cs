using UnityEngine;

public class Osi : MonoBehaviour
{
	public Vector3 movementAxis;

	private bool isDragging;

	private Vector3 lastMousePosition;

	private void OnMouseDown()
	{
		isDragging = true;
		lastMousePosition = Input.mousePosition;
	}

	private void OnMouseUp()
	{
		isDragging = false;
	}

	private void Update()
	{
		if (isDragging)
		{
			float num = (Input.mousePosition - lastMousePosition).magnitude * 0.01f;
			base.transform.parent.position += movementAxis * num;
			lastMousePosition = Input.mousePosition;
		}
	}
}
