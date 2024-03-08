using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class GlobalVideoHandler : MonoBehaviour
{
    public static VideoPlayer ActivePlayer
    {
        get
        {
            return instance.activePlayer;
        }
    }
    public static GlobalVideoHandler instance;
    [SerializeField]
    protected VideoPlayer video;
    [SerializeField]
    protected VideoPlayer videoalt;
    protected VideoPlayer activePlayer;
    protected VideoPlayer altPlayer
    {
        get
        {
            if (activePlayer == video) return videoalt;
            else return video;
        }
    }
    public string baseFolder = "";
    public static UnityEvent<VideoPlayer, double> onPrepareComplete = new UnityEvent<VideoPlayer, double>();
    public static UnityEvent<VideoPlayer> onVideoFinished = new UnityEvent<VideoPlayer>();
    Dictionary<string, RenderTexture> resRTs = new Dictionary<string, RenderTexture>();

    protected double seekTo = 0;
    public static double SeekTo
    {
        get
        {
            return instance.seekTo;
        }
        set
        {
            instance.seekTo = value;
        }
    }

    public static VideoPlayer instancePlayer
    {
        get
        {
            return instance.video;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        };
        video.errorReceived += Video_errorReceived;
        video.prepareCompleted += Video_prepareCompleted;
        video.loopPointReached += Video_loopPointReached;
        video.seekCompleted += Video_seekComplete;
        videoalt.errorReceived += Video_errorReceived;
        videoalt.prepareCompleted += Video_prepareCompleted;
        videoalt.loopPointReached += Video_loopPointReached;
        videoalt.seekCompleted += Video_seekComplete;
        activePlayer = video;
    }

    private RenderTexture getRenderTextureForResolution(int h, int w)
    {
        string key = $"{h}-{w}";
        RenderTexture rtOut;
        if (!resRTs.TryGetValue(key, out rtOut))
        {
            rtOut = new RenderTexture(w, h, 0);
            resRTs.Add(key, rtOut);
        }
        return rtOut;
    }

    private void Video_seekComplete(VideoPlayer source)
    {
        if (source.targetTexture == null
         || source.targetTexture.height != source.height || source.targetTexture.width != source.width
         )
        {
            source.targetTexture = getRenderTextureForResolution((int)source.height, (int)source.width);
        }
        else
        {
            // if (source.targetTexture.height != source.height)
            // {
            //     source.targetTexture.height = (int)source.height;
            // }
            // if (source.targetTexture.width != source.width)
            // {
            //     source.targetTexture.width = (int)source.width;
            // }
        }
        onPrepareComplete.Invoke(source, seekTo);
    }

    private void Video_loopPointReached(VideoPlayer source)
    {
        if (!video.isLooping)
            onVideoFinished.Invoke(source);
    }

    public static void PlayVideo(string URL, bool loop = false)
    {
        instance.activePlayer.Stop();
        instance.activePlayer = instance.altPlayer;
        instance.activePlayer.url = pathResolver(URL);
        instance.activePlayer.Prepare();
        instance.activePlayer.isLooping = loop;
    }

    public static string pathResolver(string URL)
    {
        return $"{Application.persistentDataPath}/{instance.baseFolder}/{URL}.mp4";
    }

    public static void StopVideo(string URL)
    {
        if (instance.activePlayer.url == pathResolver(URL))
        {
            instance.activePlayer.Stop();
            instance.activePlayer = instance.altPlayer;
        }
    }

    private void Video_prepareCompleted(VideoPlayer source)
    {
        if (seekTo > 0)
        {
            instance.video.time = seekTo;
            seekTo = 0;
            return;
        }
        if (source.targetTexture == null || source.targetTexture.height != source.height || source.targetTexture.width != source.width)
        {
            source.targetTexture = getRenderTextureForResolution((int)source.height, (int)source.width);
        }
        onPrepareComplete.Invoke(source, 0);
    }



    private void Video_errorReceived(VideoPlayer source, string message)
    {
        Debug.LogError("VIDEO FAILED TO PLAY!");
        Debug.LogError(message);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
