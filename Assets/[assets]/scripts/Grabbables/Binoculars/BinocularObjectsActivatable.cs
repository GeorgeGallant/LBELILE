using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularObjectsActivatable : BaseSceneActivatable
{
    public GameObject binocularObjects;
    public override void activateModifiers()
    {
        base.activate();
        BinocularGrabbable binocular = ScenarioManager.gameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>();
        binocular.zoomObjects = binocularObjects;
    }
}
