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
    float blendTarget = 0f;
    float blendAmount = 0f;
    public float blendRate = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = gameObject.GetComponentInParent<XRController>();

        Debug.Log(controller.name);

    }

    // Update is called once per frame
    void Update()
    {

        blendAmount += (blendTarget - blendAmount) * blendRate;

        animator.SetFloat("Blend", blendAmount);

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
                //Debug.Log("Closed Hand");
                blendTarget = 1f;
                return;
            }

            if (trigger)
            {
                //Debug.Log("Press Trigger");
                blendTarget = 1f;
                //animator.Play("Base Layer.Index Close", 0, 2f);
                return;
            }

            if (grab)
            {
                //Debug.Log("Press Grip");
                blendTarget = 0.5f;
                //animator.Play("Base Layer.Index Point", 0, 2f);
                return;
            }

            if (!grab && !trigger)
            {
                //Debug.Log("Open Hand");
                blendTarget = 0f;
                //animator.Play("Base Layer.Idle", 0, 2f);
            }
        }


    }
}