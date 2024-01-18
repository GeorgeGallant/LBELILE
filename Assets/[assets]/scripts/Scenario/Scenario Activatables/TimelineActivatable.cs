using UnityEngine.Playables;

public class TimelineActivatable : BaseSceneActivatable
{
    protected override void StartSetup()
    {
        director = gameObject.GetComponent<PlayableDirector>();
    }
    PlayableDirector director;
    public override void activateModifiers()
    {
        director.extrapolationMode = scenarioActivatable.loopVideo ? DirectorWrapMode.Loop : DirectorWrapMode.None;
        director.Play();
    }
    public override void deactivateModifiers()
    {
        base.deactivate();
        director.Stop();
    }
}