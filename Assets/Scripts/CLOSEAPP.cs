using System.Collections;
using UnityEngine;

public class CLOSEAPP : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(CloseGameAfterDelay(10f));
	}

	private IEnumerator CloseGameAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Application.Quit();
	}
}
