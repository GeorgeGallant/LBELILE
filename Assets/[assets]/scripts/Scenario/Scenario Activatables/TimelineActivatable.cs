using UnityEngine.Playables;

public class TimelineActivatable : BaseSceneActivatable
{
    public bool syncWithVideo = true;
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
            var video = gameObject.GetComponentInParent<VideoLoaderV2>();
            video.onVideoPrepared.AddListener(director.Play);
            video.videoStopped.AddListener(director.Stop);
        }
    }
    public override void deactivateModifiers()
    {
        director.Stop();
        if (syncWithVideo)
        {
            var video = gameObject.GetComponentInParent<VideoLoaderV2>();
            video.onVideoPrepared.RemoveListener(director.Play);
            video.videoStopped.RemoveListener(director.Stop);
        }
    }
}