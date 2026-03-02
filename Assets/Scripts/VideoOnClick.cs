using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoOnClick : MonoBehaviour
{
	private Material originalMaterial;

	private VideoPlayer videoPlayer;

	private bool isVideoPlaying;

	public VideoClip videoClip;

	private void Start()
	{
		videoPlayer = GetComponent<VideoPlayer>();
		originalMaterial = GetComponent<Renderer>().material;
		videoPlayer.clip = videoClip;
		videoPlayer.playOnAwake = false;
		videoPlayer.isLooping = true;
		RenderTexture renderTexture = new RenderTexture(256, 256, 0);
		videoPlayer.targetTexture = renderTexture;
		GetComponent<Renderer>().material.mainTexture = renderTexture;
	}

	private void OnMouseDown()
	{
		if (isVideoPlaying)
		{
			videoPlayer.Stop();
			GetComponent<Renderer>().material = originalMaterial;
			isVideoPlaying = false;
		}
		else
		{
			videoPlayer.Play();
			isVideoPlaying = true;
		}
	}
}
