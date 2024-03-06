using UnityEngine;
using UnityEngine.Events;

public class ActivateWithSceneActivatable : BaseSceneActivatable
{
    protected override void StartSetup()
    {
        gameObject.SetActive(false);
    }
    public override void activateModifiers()
    {
        gameObject.SetActive(true);
    }
    public override void deactivateModifiers()
    {
        gameObject.SetActive(false);
    }
}