using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseSceneActivatable : MonoBehaviour
{
    public UnityEvent activateEvent = new UnityEvent();
    public BaseScene activateScenario;
    bool startRan = false;
    protected bool scenarioActive
    {
        get
        {
            return scenarioActivatable.isActive;
        }
    }
    protected virtual void Start()
    {
        if (startRan) return;
        startRan = true;
    }
    protected void activateNextScenario()
    {
        activateScenario.startScene();
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
