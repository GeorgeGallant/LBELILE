using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BinocularActivatable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

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

    // Update is called once per frame
    void Update()
    {

    }
}
