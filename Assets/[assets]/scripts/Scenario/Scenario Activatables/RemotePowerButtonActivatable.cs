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
        if (remote)
            remote.powerButtonEvent.AddListener(powerActivate);
    }
    void OnDisable()
    {
        if (remote)
            remote.powerButtonEvent.RemoveListener(powerActivate);
    }

    private void powerActivate()
    {
        activateEvent.Invoke();
        activateNextScene();
    }
}