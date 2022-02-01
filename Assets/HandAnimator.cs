using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    XRController controller;
    public string controllerName;
    Animator animator;
    bool trigger = false;
    bool grab = false;
    bool lastPressedTrigger = false;
    bool lastPressedGrab = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject parentObj = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        Debug.Log(parentObj.name);
        if(parentObj != null)
        {
            Debug.Log("Found Controller Parent");
            controller = parentObj.GetComponent<XRController>();
            Debug.Log(controller);
        }
         
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(controller.name + controllerName);    
        if (controller && controller.name == controllerName)
        {

            controller.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool triggerPressed, controller.axisToPressThreshold);
            controller.inputDevice.IsPressed(InputHelpers.Button.Grip, out bool grabPressed, controller.axisToPressThreshold);
            if (triggerPressed != lastPressedTrigger)
            {
                trigger = triggerPressed;
            }

            if (grabPressed != lastPressedGrab)
            {
                grab = grabPressed;
                
            }

            lastPressedTrigger = triggerPressed;
            lastPressedGrab = grabPressed;

            if (grab && trigger)
            {
                Debug.Log("Closed Hand");
                animator.Play("Base Layer.Index Close", 0, 2f);
                return;
            }

            if (trigger)
            {
                Debug.Log("Press Trigger");
                animator.Play("Base Layer.Index Close", 0, 2f);
                return;
            }

            if (grab)
            {
                Debug.Log("Press Grip");
                animator.Play("Base Layer.Index Point", 0, 2f);
                return;
            }

            if (!grab && !trigger)
            {
                Debug.Log("Open Hand");
                animator.Play("Base Layer.Idle", 0, 2f);
            }
        }


    }
}