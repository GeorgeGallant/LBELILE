using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularEventActivatable : BaseSceneActivatable
{
    public bool allowActivate = false;

    public void setAllowActivate(bool allow)
    {
        if (sceneActive)
            allowActivate = allow;
    }
    public override void activateModifiers()
    {
        base.activate();
        allowActivate = false;
        ScenarioManager.gameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>().onReleased.AddListener(released);
    }

    private void released()
    {
        if (sceneActive && allowActivate)
            activateNextScene();
    }

    public override void deactivateModifiers()
    {
        base.deactivate();
        ScenarioManager.gameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>().onReleased.RemoveListener(released);
    }
}
