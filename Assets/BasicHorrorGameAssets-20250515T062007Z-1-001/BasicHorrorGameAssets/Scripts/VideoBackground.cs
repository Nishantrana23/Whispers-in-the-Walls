using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoBackground : MonoBehaviour
{
    public RawImage rawImage;       // Assign this in inspector
    public VideoPlayer videoPlayer; // Assign this in inspector
    public AudioSource audioSource; // Optional, if your video has sound

    void Start()
    {
        // Prepare the video
        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        // Set the RawImage texture to the video player's texture
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();

        // If your video has audio
        if (audioSource != null)
        {
            videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            videoPlayer.SetTargetAudioSource(0, audioSource);
            audioSource.Play();
        }
    }
}
