using UnityEngine;

public class Sound : MonoBehaviour
{
	public AudioSource soundPlay;

	public void PlayThisSoundEffect()
	{
		soundPlay.Play();
	}
}
