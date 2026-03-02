using UnityEngine;

public class SCALE : MonoBehaviour
{
	private bool isSelected;

	public float scaleSpeed = 0.05f;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			CheckObjectSelection();
		}
		if (isSelected && Input.GetKey(KeyCode.S))
		{
			float num = scaleSpeed * Time.deltaTime * 50f;
			base.transform.localScale += new Vector3(num, num, 0f);
		}
	}

	private void CheckObjectSelection()
	{
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo))
		{
			if (hitInfo.transform == base.transform)
			{
				isSelected = true;
			}
			else
			{
				isSelected = false;
			}
		}
	}
}
