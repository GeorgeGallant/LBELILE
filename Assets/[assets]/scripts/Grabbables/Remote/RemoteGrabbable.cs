using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class RemoteGrabbable : MonoBehaviour
{
    XRHandedGrabInteractable interactable;
    bool held = false;
    bool powerPressed = false;
    private Transform tipTransform;
    [SerializeField]
    private Collider powerButtonCollider;
    public UnityEvent powerButtonEvent = new UnityEvent();

    protected void Start()
    {
        interactable = gameObject.GetComponent<XRHandedGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);

    }

    void setTip(IXRSelectInteractor interactor)
    {
        GameObject indexTip = interactor.Equals(GlobalPlayer.globalLeftController) ? GameObject.FindGameObjectWithTag("IndexTipRight") : GameObject.FindGameObjectWithTag("IndexTipLeft");
        if (!indexTip)
        {
            Debug.LogError("No fingertip found!");
            return;
        }
        tipTransform = indexTip.transform;
    }

    private void PickedUp(SelectEnterEventArgs args = null)
    {
        held = true;
        setTip(args.interactorObject);
    }
    private void LetGo(SelectExitEventArgs args = null)
    {
        held = false;
        tipTransform = null;
    }

    private void Update()
    {
        if (!held || !tipTransform) return;

        if (powerButtonCollider.bounds.Contains(tipTransform.position))
        {
            if (powerPressed) return;
            powerPressed = true;
            powerButtonEvent.Invoke();

        }
        else if (powerPressed) powerPressed = false;
    }

}
