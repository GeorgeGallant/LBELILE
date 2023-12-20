using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class NotepadGrabbable : MonoBehaviour
{
    XRHandedGrabInteractable interactable;
    public XRHandedGrabInteractable penGrabbable;
    public Transform selectorTip;
    private bool held = false;

    public NotepadLine[] lines;
    NotepadLine currentSelection;
    Dictionary<NotepadLine, UnityEvent> lineEvents = new Dictionary<NotepadLine, UnityEvent>();
    protected void Start()
    {
        interactable = gameObject.GetComponent<XRHandedGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
        penGrabbable.gameObject.SetActive(false);
        lines = GetComponentsInChildren<NotepadLine>();
        interactable.activated.AddListener(activateEvent);
        penGrabbable.activated.AddListener(activateEvent);

    }

    private void activateEvent(ActivateEventArgs arg0)
    {
        if (!currentSelection) return;
        lineEvents[currentSelection].Invoke();
    }

    protected void clearNotepad()
    {
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
    public void SetLines(NotepadLineElement[] elements)
    {
        lineEvents.Clear();
        clearNotepad();
        int distance = (lines.Length / (elements.Length + 1)) - 1;
        int i = 0;
        foreach (var item in elements)
        {
            int lineIndex = (i + 1) * distance;
            Debug.Log(lineIndex);
            NotepadLine line = lines[lineIndex];
            lineEvents.Add(line, item.lineEvent);
            line.tmp.SetText(item.lineText);
            Debug.Log(item.lineText);
            line.available = true;
            i++;
        }
    }
    protected void Update()
    {
        if (!held) { if (currentSelection) { currentSelection.circle.enabled = true; currentSelection = null; } return; }
        float distanceToBeat = 5000;
        NotepadLine newLine = null;
        foreach (var item in lines)
        {
            if (!item.available) continue;
            float distance = Vector3.Distance(item.bounds.ClosestPointOnBounds(selectorTip.position), selectorTip.position);
            if (distance < distanceToBeat) { distanceToBeat = distance; newLine = item; }
        }
        if (distanceToBeat < 0.1)
        {
            if (currentSelection == newLine) return;
            if (currentSelection)
                currentSelection.circle.enabled = false;
            currentSelection = newLine;
            currentSelection.circle.enabled = true;
        }
        else
        {
            if (currentSelection)
            {
                currentSelection.circle.enabled = false;
                currentSelection = null;
            }
        }
    }
}
