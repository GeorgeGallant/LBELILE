using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoLoaderV2 : MonoBehaviour
{
    public string videoURL = "";
    public string targetMaterialProperty = "";
    public GameObject targetGameObject;

    public void playVideo()
    {
        GlobalVideoHandler.PlayVideo(videoURL);
        GlobalVideoHandler.onPrepareComplete.AddListener(prepareCompleted);
    }

    public void stopVideo()
    {
        GlobalVideoHandler.StopVideo(videoURL);
    }

    void prepareCompleted(RenderTexture renderTexture)
    {
        GlobalVideoHandler.onPrepareComplete.RemoveListener(prepareCompleted);
        (targetGameObject == null ? gameObject : targetGameObject).GetComponent<Renderer>().material.SetTexture(targetMaterialProperty == "" ? "_BaseMap" : targetMaterialProperty, renderTexture);
    }
}
