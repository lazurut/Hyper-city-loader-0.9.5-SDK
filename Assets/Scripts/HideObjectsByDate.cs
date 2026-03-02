using System;
using UnityEngine;

public class HideObjectsByDate : MonoBehaviour
{
	public GameObject[] objectsToHide;

	private DateTime startDate = new DateTime(DateTime.Now.Year, 10, 25);

	private DateTime endDate = new DateTime(DateTime.Now.Year, 10, 31);

	private void Start()
	{
		DateTime now = DateTime.Now;
		if (now >= startDate && now <= endDate)
		{
			ShowObjects();
		}
		else if (now > endDate)
		{
			HideObjects();
		}
	}

	private void ShowObjects()
	{
		GameObject[] array = objectsToHide;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
	}

	private void HideObjects()
	{
		GameObject[] array = objectsToHide;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: false);
		}
	}
}
