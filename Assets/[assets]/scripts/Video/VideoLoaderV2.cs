using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoLoaderV2 : MonoBehaviour
{
    public string videoURL = "";
    public string targetMaterialProperty = "";
    public GameObject targetGameObject;
    public bool currentlyPlaying = false;
    public UnityEvent<bool> playStateChanged = new UnityEvent<bool>();

    void Start()
    {
        GlobalVideoHandler.onPrepareComplete.AddListener(prepareCompleted);
    }

    public void playVideo()
    {
        GlobalVideoHandler.PlayVideo(videoURL);

    }

    public void stopVideo()
    {
        changePlayState(false);
        GlobalVideoHandler.StopVideo(videoURL);
    }

    void changePlayState(bool newState)
    {
        if (currentlyPlaying != newState)
            playStateChanged.Invoke(newState);
        currentlyPlaying = newState;
    }

    void prepareCompleted(VideoPlayer source)
    {
        // GlobalVideoHandler.onPrepareComplete.RemoveListener(prepareCompleted);
        Debug.Log(source);
        if (source.url != GlobalVideoHandler.pathResolver(videoURL)) { changePlayState(false); return; }
        changePlayState(true);
        (targetGameObject == null ? gameObject : targetGameObject).GetComponent<Renderer>().material.SetTexture(targetMaterialProperty == "" ? "_BaseMap" : targetMaterialProperty, source.targetTexture);
    }
}
