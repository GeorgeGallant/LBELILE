public class TeleportActivatable : BaseSceneActivatable
{
    public override void activateModifiers()
    {
        GlobalPlayer.AddTeleportUser(this);
    }
    public override void deactivateModifiers()
    {
        GlobalPlayer.RemoveTeleportUser(this);
    }
    public void Selected()
    {
        activateEvent.Invoke();
        activateNextScene();
    }
}