using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public string videoURL;
    [HideInInspector]
    public VideoLoaderV2 videoLoader;
    public ScenarioObject[] scenarioObjects;
    public BaseScene videoFinishedScenario;
    public bool loopVideo = false;
    public bool isActive
    {
        get
        {
            return ScenarioManager.instance.activeScenario == this;
        }
    }
    void Start()
    {
        var activatables = GetComponentsInChildren<BaseSceneActivatable>();
        foreach (var item in activatables)
        {
            item.setOwnerScenario(this);
        }
        if (videoURL != string.Empty)
        {
            videoLoader = gameObject.AddComponent<VideoLoaderV2>();
            videoLoader.loop = loopVideo;
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