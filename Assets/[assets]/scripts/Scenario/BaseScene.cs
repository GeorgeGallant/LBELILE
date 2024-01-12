using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;

[ExecuteAlways]
public class BaseScene : MonoBehaviour
{
    public string videoURL;
    [HideInInspector]
    public VideoLoaderV2 videoLoader;
    public SceneObject[] scenarioObjects;
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
        if (!Application.isPlaying) return;
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
        Debug.Log("activating scenario!");
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
#if UNITY_EDITOR
    void setSphereTexture()
    {
        try
        {

            Texture2D debugTexture = new Texture2D(1024, 512);
            var manager = FindAnyObjectByType<ScenarioManager>();
            var videoLoader = FindAnyObjectByType<GlobalVideoHandler>();
            var bytes = System.IO.File.ReadAllBytes($"{Application.persistentDataPath}/{videoLoader.baseFolder}/thumbs/{videoURL}.jpg");
            ImageConversion.LoadImage(debugTexture, bytes);
            manager.videoSphere.GetComponent<Renderer>().sharedMaterial.SetTexture("_BaseMap", debugTexture);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

    }
    GameObject previousSelection = null;
#endif

    void Update()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;
        if (UnityEditor.Selection.activeGameObject != previousSelection && UnityEditor.Selection.activeGameObject == gameObject)
        {
            setSphereTexture();
        }
        previousSelection = UnityEditor.Selection.activeGameObject;

#endif
    }

}