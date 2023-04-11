using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;

public class StartPlayback : MonoBehaviour
{
    public VideoPlayer video;
    public List<AudioSource> audio;

    public void StartVideo()
    {
        gameObject.SetActive(false);
        video.Play();
        foreach (AudioSource sound in audio)
        {
            sound.Play(0);
        }
    }
}
