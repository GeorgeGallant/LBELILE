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
    public bool useLookEvents = false;
    bool looked = false;
    bool releaseEvent = false;
    public float activateLookAngle = 15;
    public float lookTime = 3;
    public float currentAngle;
    float lookingTime = 0;
    public UnityEvent lookEvent = new UnityEvent();
    public UnityEvent releaseLookEvent = new UnityEvent();
    public UnityEvent<bool> onLookEvent = new UnityEvent<bool>();
    Transform lookTransform;

    void Start()
    {
        lookTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        if (comboActivator)
            onLookEvent.AddListener(changeActivationState);
    }

    public override void OnHeldUp()
    {
        checkForLook = true;
        base.OnHeldUp();
    }

    public override void OnReleased()
    {
        checkForLook = false;
        onLookEvent.Invoke(false);
        if (releaseEvent)
        {
            releaseLookEvent.Invoke();
            releaseEvent = false;
        }
        base.OnReleased();
    }
    void Update()
    {

        float lookAngle = Vector3.Angle(lookTransform.forward, Vector3.Normalize(transform.position - lookTransform.position));
        bool frameLooking = lookAngle < activateLookAngle;

        currentAngle = lookAngle;

        if (looking != frameLooking)
        {
            onLookEvent.Invoke(frameLooking);
        }

        looking = frameLooking;

        if (!useLookEvents || (once && looked) || !checkForLook)
        {
            // if (looking) onLookEvent.Invoke(false);
            // looking = false;
            return;
        }

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