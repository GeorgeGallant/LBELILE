using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseSceneActivatable : BaseActivatable
{
    public UnityEvent activateEvent = new UnityEvent();
    public BaseScene activateScene;

    protected void activateNextScene()
    {
        if (activateScene)
        {
            activateScene.startScene();
        }
    }
}
