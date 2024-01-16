using UnityEngine.Playables;

public class TimelineActivatable : BaseSceneActivatable
{
    protected override void StartSetup()
    {
        director = gameObject.GetComponent<PlayableDirector>();
    }
    PlayableDirector director;
    public override void activate()
    {
        base.activate();
        director.extrapolationMode = scenarioActivatable.loopVideo ? DirectorWrapMode.Loop : DirectorWrapMode.None;
        director.Play();
    }
    public override void deactivate()
    {
        base.deactivate();
        director.Stop();
    }
}