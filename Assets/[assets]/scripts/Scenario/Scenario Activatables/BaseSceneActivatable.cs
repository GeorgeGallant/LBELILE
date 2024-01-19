using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseSceneActivatable : MonoBehaviour
{
    public UnityEvent activateEvent = new UnityEvent();
    public BaseScene activateScene;
    bool startRan = false;
    protected bool sceneActive
    {
        get
        {
            if (activatableOwner)
                return activatableOwner.isActive;
            else return false;
        }
    }
    protected virtual void Start()
    {
        if (startRan) return;
        startRan = true;
        StartSetup();
    }
    protected virtual void StartSetup() { }
    protected void activateNextScene()
    {
        if (activateScene)
        {
            activateScene.startScene();
        }
    }
    bool canActivate
    {
        get
        {
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

    public void setOwnerScenario(BaseScene owner, bool overrideOwner = false)
    {
        if (activatableOwner && overrideOwner)
        {
            activatableOwner = owner;
        }
        else activatableOwner = owner;
    }
    internal BaseScene activatableOwner;
}
