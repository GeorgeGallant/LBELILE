using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class GlobalVideoHandler : MonoBehaviour
{
    public static GlobalVideoHandler instance;
    VideoPlayer video;
    public string baseFolder = "";
    public static UnityEvent<VideoPlayer> onPrepareComplete = new UnityEvent<VideoPlayer>();

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
        video = GetComponent<VideoPlayer>();
        video.errorReceived += Video_errorReceived;
        video.prepareCompleted += Video_prepareCompleted;
    }

    public static void PlayVideo(string URL, bool playImmediately = true, bool loop = false)
    {
        instance.video.url = pathResolver(URL);
        instance.video.Prepare();
        instance.video.isLooping = loop;
    }

    public static string pathResolver(string URL)
    {
        return $"{Application.persistentDataPath}/{instance.baseFolder}/{URL}.mp4";
    }

    public static void StopVideo(string URL)
    {
        if (instance.video.url == pathResolver(URL))
            instance.video.Stop();
    }

    private void Video_prepareCompleted(VideoPlayer source)
    {
        if (source.targetTexture == null || source.targetTexture.height != source.height || source.targetTexture.width != source.width)
        {
            source.targetTexture = new RenderTexture((int)source.width, (int)source.height, 0);
        }
        Debug.Log(source);
        onPrepareComplete.Invoke(source);
    }



    private void Video_errorReceived(VideoPlayer source, string message)
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
}
