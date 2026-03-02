using UnityEngine;

public class Pokaz : MonoBehaviour
{
	public GameObject targetObject;

	public Color highlightColor = Color.green;

	private Color originalColor;

	private bool isHighlighted;

	private void Start()
	{
		if (targetObject != null)
		{
			Renderer component = targetObject.GetComponent<Renderer>();
			if (component != null)
			{
				originalColor = component.material.color;
			}
		}
		else
		{
			Debug.LogError("Target Object не установлен в инспекторе!");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == targetObject)
		{
			Vector3 position = targetObject.transform.position;
			Debug.Log($"Объект находится по координатам: {position}");
			HighlightObject(highlight: true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == targetObject)
		{
			HighlightObject(highlight: false);
		}
	}

	private void HighlightObject(bool highlight)
	{
		if (!(targetObject != null))
		{
			return;
		}
		Renderer component = targetObject.GetComponent<Renderer>();
		if (component != null)
		{
			if (highlight)
			{
				component.material.color = highlightColor;
				isHighlighted = true;
			}
			else
			{
				component.material.color = originalColor;
				isHighlighted = false;
			}
		}
	}
}
