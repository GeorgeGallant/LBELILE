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
    public BaseSceneActivatable[] activatables;
    bool ranStart = false;
    public bool isActive
    {
        get
        {
            return ScenarioManager.instance.activeScenario == this;
        }
    }
    protected void Start()
    {
        if (ranStart) return;
        ranStart = true;
        var activatables = GetComponentsInChildren<BaseSceneActivatable>();
        foreach (var item in activatables)
        {
            item.setOwnerScenario(this);
        }
        this.activatables = activatables;
        if (videoURL != string.Empty)
        {
            videoLoader = gameObject.AddComponent<VideoLoaderV2>();
            videoLoader.loop = loopVideo;
            videoLoader.videoURL = videoURL;
            videoLoader.targetGameObject = ScenarioManager.VideoSphere;
            videoLoader.onVideoFinished.AddListener(videoFinished);
        }

    }
    public virtual void startScene()
    {
        Start();
        if (ScenarioManager.ActiveScenario)
            ScenarioManager.ActiveScenario.deactivateScene();
        ScenarioManager.ActiveScenario = this;
        if (videoLoader)
        {
            videoLoader.onVideoPrepared.AddListener(activateScene);
            videoLoader.playVideo();
        }
        else activateScene();

    }

    void activateScene()
    {
        ScenarioManager.enableScenarioObjects(scenarioObjects);
        foreach (var item in activatables)
        {
            item.activate();
        }
    }

    protected void deactivateScene()
    {
        foreach (var item in activatables)
        {
            item.deactivate();
        }
    }

    void videoFinished()
    {
        videoFinishedScenario.startScene();
    }

}