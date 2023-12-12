using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScenario : MonoBehaviour
{
    public string videoURL;
    VideoLoaderV2 videoLoader;
    public ScenarioObject[] scenarioObjects;
    void Start()
    {
        if (videoURL != string.Empty) { videoLoader = gameObject.AddComponent<VideoLoaderV2>(); videoLoader.videoURL = videoURL; }

    }
    public virtual void startScenario()
    {
        if (videoLoader) videoLoader.playVideo();
    }
}