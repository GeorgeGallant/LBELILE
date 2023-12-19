using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NotepadGrabbable : MonoBehaviour
{
    XRHandedGrabInteractable interactable;
    public XRHandedGrabInteractable penGrabbable;
    public Transform selectorTip;
    private bool held = false;

    public NotepadLine[] lines;
    NotepadLine currentSelection;
    protected void Start()
    {
        interactable = gameObject.GetComponent<XRHandedGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
        penGrabbable.gameObject.SetActive(false);
        lines = GetComponentsInChildren<NotepadLine>();
        foreach (var item in lines)
        {
            item.clear();
        }
    }

    protected void LetGo(SelectExitEventArgs args = null)
    {
        held = false;
        // var otherHand = GlobalPlayer.GetOtherHand(args.interactorObject as XRDirectInteractor);
        // args.manager.SelectExit(otherHand as IXRSelectInteractor, penGrabbable);
        penGrabbable.forceHeld = false;
        penGrabbable.gameObject.SetActive(false);
    }
    protected void PickedUp(SelectEnterEventArgs args = null)
    {
        held = true;
        penGrabbable.gameObject.SetActive(true);
        penGrabbable.forceHeld = true;
        var otherHand = GlobalPlayer.GetOtherHand(args.interactorObject as XRDirectInteractor);
        args.manager.SelectEnter(otherHand as IXRSelectInteractor, penGrabbable);
    }
    protected void Update()
    {
        if (!held) return;
        float distanceToBeat = 5000;
        foreach (var item in lines)
        {
            if (!item.available) continue;
        }
    }
}
