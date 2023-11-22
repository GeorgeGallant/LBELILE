using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BinocularLookActivatable : BinocularActivatable
{
    bool checkForLook = false;
    public bool looking = false;
    public bool once = true;
    bool looked = false;
    bool releaseEvent = false;
    public float activateLookAngle = 15;
    public float lookTime = 3;
    float lookingTime = 0;
    public UnityEvent lookEvent = new UnityEvent();
    public UnityEvent releaseLookEvent = new UnityEvent();
    Transform lookTransform;

    void Start()
    {
        lookTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public override void OnHeldUp()
    {
        checkForLook = true;
        base.OnHeldUp();
    }

    public override void OnReleased()
    {
        checkForLook = false;
        if (releaseEvent)
        {
            releaseLookEvent.Invoke();
            releaseEvent = false;

        }
        base.OnReleased();
    }
    void Update()
    {
        if ((once && looked) || !checkForLook)
        {
            looking = false;
            return;
        }
        float lookAngle = Vector3.Angle(lookTransform.forward, Vector3.Normalize(transform.position - lookTransform.position));
        looking = lookAngle < activateLookAngle;

        if (looking)
        {
            lookingTime += Time.deltaTime;
        }
        else
        {
            lookingTime = 0;
        }

        if (lookingTime >= lookTime)
        {
            if (once) looked = true;
            else looked = false;

            lookingTime = 0;
            releaseEvent = true;
            lookEvent.Invoke();
        }




    }
}