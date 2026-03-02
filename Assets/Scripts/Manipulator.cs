using UnityEngine;

public class Manipulator : MonoBehaviour
{
	public bool gizmosActive;

	private Vector3 offset;

	private Transform selectedAxis;

	private void OnDrawGizmos()
	{
		if (gizmosActive)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.right);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.up);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(base.transform.position, base.transform.position + Vector3.forward);
		}
	}

	private void OnMouseDown()
	{
		if (gizmosActive && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo))
		{
			selectedAxis = hitInfo.transform;
			offset = base.transform.position - hitInfo.point;
		}
	}

	private void OnMouseDrag()
	{
		if (gizmosActive && !(selectedAxis == null))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (new Plane(selectedAxis.up, base.transform.position).Raycast(ray, out var enter))
			{
				Vector3 point = ray.GetPoint(enter);
				base.transform.position = point + offset;
			}
		}
	}

	public void SetGizmosActive(bool active)
	{
		gizmosActive = active;
	}
}
