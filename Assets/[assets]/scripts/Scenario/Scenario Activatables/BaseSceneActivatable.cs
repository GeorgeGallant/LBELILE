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
            if (scenarioActivatable)
                return scenarioActivatable.isActive;
            else return false;
        }
    }
    protected virtual void Start()
    {
        if (startRan) return;
        startRan = true;
    }
    protected void activateNextScene()
    {
        if (activateScene)
        {
            activateScene.startScene();
        }
    }
    public virtual void activate()
    {
        Start();
    }
    public virtual void deactivate()
    {

    }
    public void setOwnerScenario(BaseScene owner, bool overrideOwner = false)
    {
        if (scenarioActivatable && overrideOwner)
        {
            scenarioActivatable = owner;
        }
        else scenarioActivatable = owner;
    }
    protected BaseScene scenarioActivatable;
}
