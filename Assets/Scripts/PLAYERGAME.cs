using UnityEngine;

public class PLAYERGAME : MonoBehaviour
{
	public GameObject player1;

	public GameObject player2;

	public MeshRenderer player1Model;

	public MeshRenderer player2Model;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchToPlayer1();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SwitchToPlayer2();
		}
	}

	private void SwitchToPlayer1()
	{
		player1.SetActive(value: true);
		player2.SetActive(value: false);
		player1Model.enabled = true;
		player2Model.enabled = false;
	}

	private void SwitchToPlayer2()
	{
		player1.SetActive(value: false);
		player2.SetActive(value: true);
		player1Model.enabled = false;
		player2Model.enabled = true;
	}
}
