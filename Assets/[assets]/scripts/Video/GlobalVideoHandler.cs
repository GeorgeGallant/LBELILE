using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class GlobalVideoHandler : MonoBehaviour
{
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

    private void Video_seekComplete(VideoPlayer source)
    {
        if (source.targetTexture == null || source.targetTexture.height != source.height || source.targetTexture.width != source.width)
        {
            source.targetTexture = new RenderTexture((int)source.width, (int)source.height, 0);
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
            source.targetTexture = new RenderTexture((int)source.width, (int)source.height, 0);
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
        if (activePlayer.isPaused) activePlayer.Play();
    }
}
