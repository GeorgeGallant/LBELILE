using UnityEngine.tvOS;

public class RemotePowerButtonActivatable : BaseSceneActivatable
{
    RemoteGrabbable remote;
    protected override void StartSetup()
    {
        remote = ScenarioManager.GameObjectDictionary[ScenarioObject.Remote].GetComponent<RemoteGrabbable>();
    }
    public override void activateModifiers()
    {
        remote.powerButtonEvent.AddListener(powerActivate);
    }
    public override void deactivateModifiers()
    {
        remote.powerButtonEvent.RemoveListener(powerActivate);
    }

    private void powerActivate()
    {
        activateEvent.Invoke();
        activateNextScene();
    }
}