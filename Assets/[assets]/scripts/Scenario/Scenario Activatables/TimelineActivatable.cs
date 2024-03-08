using UnityEngine;
using UnityEngine.Playables;

public class TimelineActivatable : BaseSceneActivatable
{
    public bool syncWithVideo = true;
    VideoLoaderV2 videoLoader;
    bool videoPlaying = false;
    protected override void StartSetup()
    {
        director = gameObject.GetComponent<PlayableDirector>();
    }
    PlayableDirector director;
    public override void activateModifiers()
    {
        director.extrapolationMode = activatableOwner.loopVideo ? DirectorWrapMode.Loop : DirectorWrapMode.None;
        director.Play();
        if (syncWithVideo)
        {
            if (!videoLoader) videoLoader = gameObject.GetComponentInParent<VideoLoaderV2>();
            videoLoader.onVideoPrepared.AddListener(directorPlayWithSeek);
            videoLoader.videoStopped.AddListener(director.Stop);
        }
    }
    void directorPlayWithSeek(double seek)
    {
        director.Play();
        if (syncWithVideo) videoPlaying = true;
        if (seek != 0)
            director.time = seek;
    }
    public override void deactivateModifiers()
    {
        director.Stop();
        if (syncWithVideo)
        {
            var video = gameObject.GetComponentInParent<VideoLoaderV2>();
            videoPlaying = false;
            video.onVideoPrepared.RemoveListener(directorPlayWithSeek);
            video.videoStopped.RemoveListener(director.Stop);
        }
    }
    void Update()
    {
        if (syncWithVideo && videoPlaying)
        {
            if (Mathf.Abs((float)(director.time - GlobalVideoHandler.ActivePlayer.time)) > 0.1)
            {
                if (GlobalVideoHandler.ActivePlayer.url != GlobalVideoHandler.pathResolver(videoLoader.videoURL)) { videoPlaying = false; director.Stop(); return; }
                director.time = GlobalVideoHandler.ActivePlayer.time;
                director.Play();
            }
        }
    }
}