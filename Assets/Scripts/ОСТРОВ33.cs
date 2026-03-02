using UnityEngine;
using UnityEngine.SceneManagement;

public class ОСТРОВ33 : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("остров 3");
		}
	}
}
