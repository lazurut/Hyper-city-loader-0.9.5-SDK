using UnityEngine;
using UnityEngine.SceneManagement;

public class MENUTELEPORT : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("Menu");
		}
	}
}
