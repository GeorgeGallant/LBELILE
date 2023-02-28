using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
public class VideoLoader : MonoBehaviour
{
    VideoPlayer video;

    public string fileName;
    public AudioSource[] audioClips = null;

    void Awake()
    {
        PauseAllClips();
        video = FindObjectOfType<VideoPlayer>();
        video.errorReceived += Video_errorReceived;
        video.prepareCompleted += Video_prepareCompleted;

        video.url = Application.persistentDataPath + "/" + fileName + ".mp4";
        video.Prepare();

    }

    private void Video_prepareCompleted(VideoPlayer source)
    {
        video.frame = 0;
        video.Play();
        foreach (AudioSource clip in audioClips)
        {
            clip.time = (float)video.time;
            clip.Play();
        }
    }

    private void PauseAllClips()
    {
        foreach (AudioSource clip in audioClips)
        {
            clip.Stop();
        }
    }

    private void Video_errorReceived(VideoPlayer source, string message) 
    {

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                Debug.Log("/storage/emulated/0/Android/data/"+Application.identifier+"/files");
                break;
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                Debug.Log("%userprofile%\\AppData\\LocalLow\\" + Application.companyName + "\\" + Application.productName);
                break;
            case RuntimePlatform.OSXEditor:
                Debug.Log("~/Library/Application Support/" + Application.companyName + "/" + Application.productName);
                break;
        }

        Debug.Log(Application.persistentDataPath + "/" + fileName);

    }
}
