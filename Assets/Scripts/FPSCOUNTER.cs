using UnityEngine;

public class FPSCOUNTER : MonoBehaviour
{
	public int FPS { get; private set; }

	private void Ubdate()
	{
		FPS = (int)(1f / Time.deltaTime);
	}
}
