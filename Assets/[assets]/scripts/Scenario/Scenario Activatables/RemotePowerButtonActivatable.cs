public class RemotePowerButtonActivatable : BaseSceneActivatable
{
    RemoteGrabbable remote;
    protected override void StartSetup()
    {
        remote = ScenarioManager.GameObjectDictionary[ScenarioObject.Remote].GetComponent<RemoteGrabbable>();
    }
    public override void activateModifiers()
    {
        OnEnable();
    }
    public override void deactivateModifiers()
    {
        OnDisable();
    }
    void OnEnable()
    {
        remote.powerButtonEvent.AddListener(powerActivate);
    }
    void OnDisable()
    {
        remote.powerButtonEvent.RemoveListener(powerActivate);
    }

    private void powerActivate()
    {
        activateEvent.Invoke();
        activateNextScene();
    }
}