using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class GenericScene : BaseScene
{
    public string videoURL;
    [HideInInspector]
    public VideoLoaderV2 videoLoader;
    public SceneObject[] scenarioObjects;
    public BaseScene videoFinishedScenario;
    public bool loopVideo = false;
    BaseSceneActivatable[] activatables;
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
    public override void startScene()
    {
        Debug.Log("attempting to switch scenes");
        if (ScenarioManager.Occupier)
        {
            Debug.Log("Occupier prevented scene change");
            return;
        }
        // if (ScenarioManager.ActiveScenario == this) return;



        if (ScenarioManager.AllowNewScene)
        {
            Debug.Log("Scene allowed, starting scene");
            StartScene();
        }
        else
        {
            if (ScenarioManager.AttemptOccupy(this))
            {
                Debug.Log("Awaiting occupier to go away before starting scene.");
                StartCoroutine(AwaitStart());
            }
        }
    }

    IEnumerator AwaitStart()
    {
        float timeSinceLast = 1;
        while (!ScenarioManager.AllowNewScene)
        {
            timeSinceLast -= Time.deltaTime;
            if (timeSinceLast <= 0)
            {
                Debug.Log("Awaiting occupier to go away.");
                timeSinceLast = 1;
            }
            yield return null;
        }
        StartScene();
        yield return null;
    }

    void StartScene()
    {
        Debug.Log("Starting new scene");
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

    protected void activateScene(double seek = 0)
    {
        Debug.Log("activating scenario!");
        ScenarioManager.enableScenarioObjects(scenarioObjects);
        foreach (var item in activatables)
        {
            item.activate();
        }
        videoLoader.onVideoPrepared.RemoveListener(activateScene);
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
        if (videoFinishedScenario)
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