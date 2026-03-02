using UnityEngine;

public class Melodii : MonoBehaviour
{
	public AudioSource audioSource;

	public AudioClip[] audioClips;

	private int currentClipIndex;

	private void Start()
	{
		if (audioClips.Length != 0 && audioSource != null)
		{
			PlayNextClip();
		}
	}

	private void Update()
	{
		if (!audioSource.isPlaying && currentClipIndex < audioClips.Length)
		{
			PlayNextClip();
		}
	}

	private void PlayNextClip()
	{
		if (currentClipIndex < audioClips.Length)
		{
			audioSource.clip = audioClips[currentClipIndex];
			audioSource.Play();
			currentClipIndex++;
		}
	}
}
