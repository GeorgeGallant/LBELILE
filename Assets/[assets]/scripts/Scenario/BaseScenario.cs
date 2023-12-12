using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScenario : MonoBehaviour
{
    public string videoURL;
    [HideInInspector]
    public VideoLoaderV2 videoLoader;
    public ScenarioObject[] scenarioObjects;
    public BaseScenario videoFinishedScenario;
    void Start()
    {
        if (videoURL != string.Empty)
        {
            videoLoader = gameObject.AddComponent<VideoLoaderV2>();
            videoLoader.videoURL = videoURL;
            videoLoader.targetGameObject = ScenarioManager.VideoSphere;
            videoLoader.onVideoFinished.AddListener(videoFinished);
        }

    }
    public virtual void startScenario()
    {
        ScenarioManager.ActiveScenario = this;
        if (videoLoader) videoLoader.playVideo();
        ScenarioManager.enableScenarioObjects(scenarioObjects);
    }

    void videoFinished()
    {
        videoFinishedScenario.startScenario();
    }

}