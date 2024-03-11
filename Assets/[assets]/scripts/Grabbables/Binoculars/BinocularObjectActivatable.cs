public class BinocularObjectActivatable : BaseSceneActivatable
{
    void OnEnable()
    {
        BinocularGrabbable.blockers.Remove(this);
    }
    void OnDisable()
    {
        if (sceneActive)
            BinocularGrabbable.blockers.Add(this);
    }

    public override void activateModifiers()
    {
        if (gameObject.activeInHierarchy) BinocularGrabbable.blockers.Remove(this);
        else BinocularGrabbable.blockers.Add(this);
    }
    public override void deactivateModifiers()
    {
        BinocularGrabbable.blockers.Remove(this);
    }
}