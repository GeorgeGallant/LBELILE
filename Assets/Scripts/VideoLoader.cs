using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
public class VideoLoader : MonoBehaviour
{
    VideoPlayer video;
    private string rootPath;
    private string path;
    
    public string fileName;

    void Awake()
    {
        video = FindObjectOfType<VideoPlayer>();
       // video.errorReceived += VideoPlayer_errorReceived;

        if (Application.platform == RuntimePlatform.Android)
        {
            rootPath = Application.persistentDataPath;
            path = Path.Combine(rootPath, fileName);
        }

        video.url = path;
        video.Play();
    }
}
