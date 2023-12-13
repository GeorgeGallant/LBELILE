using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScenarioActivatable : MonoBehaviour
{
    // Start is called before the first frame update
    protected bool scenarioActive
    {
        get
        {
            return scenarioActivatable.isActive;
        }
    }
    BaseScenario scenarioActivatable;
    protected virtual void Start()
    {
        scenarioActivatable = GetComponentInParent<BaseScenario>();
    }
}
