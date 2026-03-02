using UnityEngine;

public class OBJ : MonoBehaviour
{
	public GameObject gizmoPrefab;

	private GameObject activeGizmo;

	private void OnMouseDown()
	{
		if (activeGizmo == null)
		{
			activeGizmo = Object.Instantiate(gizmoPrefab, base.transform.position, Quaternion.identity);
			activeGizmo.transform.parent = base.transform;
		}
		else
		{
			Object.Destroy(activeGizmo);
		}
	}
}
