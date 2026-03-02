using UnityEngine;
using UnityEngine.SceneManagement;

public class TRIGGERISLAMD3 : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("ОСТРОВ 3");
		}
	}
}
