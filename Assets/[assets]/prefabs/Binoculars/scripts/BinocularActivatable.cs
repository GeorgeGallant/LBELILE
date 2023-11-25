using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinocularActivatable : ComboActivatable
{

    public virtual void SubscribeActivatable(BinocularGrabbable binocular)
    {
        binocular.onHeldUp.AddListener(OnHeldUp);
        binocular.onReleased.AddListener(OnReleased);
    }

    public virtual void OnHeldUp()
    {

    }
    public virtual void OnReleased()
    {

    }
}
