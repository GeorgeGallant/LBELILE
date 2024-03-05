using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinocularObjectsActivatable : BaseSceneActivatable
{
    BinocularGrabbable binocular;
    public UnityEvent heldUp;
    public UnityEvent released;
    void HeldUp()
    {
        heldUp.Invoke();
    }
    void Released()
    {
        released.Invoke();
    }
    public GameObject binocularObjects;
    public override void activateModifiers()
    {
        binocular = ScenarioManager.GameObjectDictionary[ScenarioObject.Binoculars].GetComponent<BinocularGrabbable>();
        binocular.zoomObjects = binocularObjects;
        binocular.onHeldUp.AddListener(HeldUp);
        binocular.onReleased.AddListener(Released);
    }
    public override void deactivateModifiers()
    {
        binocular.onHeldUp.RemoveListener(HeldUp);
        binocular.onReleased.RemoveListener(Released);
    }
}
