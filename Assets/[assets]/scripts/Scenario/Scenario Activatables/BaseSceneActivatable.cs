using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseSceneActivatable : MonoBehaviour
{
    public UnityEvent activateEvent = new UnityEvent();
    public BaseScene activateScenario;
    protected bool scenarioActive
    {
        get
        {
            return scenarioActivatable.isActive;
        }
    }
    protected void activate()
    {
        activateScenario.startScenario();
    }
    public void setOwnerScenario(BaseScene owner, bool overrideOwner = false)
    {
        if (scenarioActivatable && overrideOwner)
        {
            scenarioActivatable = owner;
        }
        else scenarioActivatable = owner;
    }
    BaseScene scenarioActivatable;
}
