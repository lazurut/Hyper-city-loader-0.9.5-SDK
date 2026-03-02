using UnityEngine;
using UnityEngine.SceneManagement;

public class LAVATELEPOPRT : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("LAva map");
		}
	}
}
