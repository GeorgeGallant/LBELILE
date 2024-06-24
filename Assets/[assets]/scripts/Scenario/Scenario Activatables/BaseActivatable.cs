using UnityEngine;

public class BaseActivatable : MonoBehaviour
{
    internal bool startRan = false;
    protected bool sceneActive
    {
        get
        {
            if (activatableOwner)
                return activatableOwner.isActive;
            else return false;
        }
    }
    protected void Start()
    {
        if (startRan) return;
        startRan = true;
        StartSetup();
    }
    bool canActivate
    {
        get
        {
            if (!enabled) return false;
            var modifiers = gameObject.GetComponents<BaseActivatableModifier>();
            if (modifiers.Length == 0) return true;
            for (int i = 0; i < modifiers.Length; i++)
            {
                if (!modifiers[i].activatable) return false;
            }
            return true;
        }
    }
    public void activate()
    {
        if (!canActivate) return;
        Start();
        activateModifiers();
    }
    protected virtual void StartSetup() { }
    public void setOwnerScenario(GenericScene owner, bool overrideOwner = false)
    {
        if (activatableOwner && overrideOwner)
        {
            activatableOwner = owner;
        }
        else activatableOwner = owner;
    }
    internal GenericScene activatableOwner;
    public virtual void activateModifiers()
    {

    }
    public void deactivate()
    {
        deactivateModifiers();
    }
    public virtual void deactivateModifiers()
    {

    }
}
