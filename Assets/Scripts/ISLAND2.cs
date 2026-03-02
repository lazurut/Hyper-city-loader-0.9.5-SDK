using UnityEngine;
using UnityEngine.SceneManagement;

public class ISLAND2 : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("остров 2");
		}
	}
}
