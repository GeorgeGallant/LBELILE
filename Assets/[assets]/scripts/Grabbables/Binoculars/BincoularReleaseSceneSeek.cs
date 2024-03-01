using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BincoularReleaseSceneSeek : BaseSceneActivatable
{
    public double seekTime = 0;
    public override void activateModifiers()
    {
        ScenarioManager.GameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>().onReleased.AddListener(released);
    }
    public override void deactivateModifiers()
    {
        ScenarioManager.GameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>().onReleased.RemoveListener(released);
    }
    private void released()
    {
        GlobalVideoHandler.SeekTo = seekTime;
    }
}
