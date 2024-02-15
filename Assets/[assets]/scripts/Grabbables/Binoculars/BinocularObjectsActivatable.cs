using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinocularObjectsActivatable : BaseSceneActivatable
{
    public GameObject binocularObjects;
    public override void activateModifiers()
    {
        BinocularGrabbable binocular = ScenarioManager.GameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>();
        binocular.zoomObjects = binocularObjects;
    }
}
