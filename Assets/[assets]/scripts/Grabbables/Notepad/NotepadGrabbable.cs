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
    Transform penParent;
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
        penGrabbable.selectExited.AddListener(resetPenPos);
        lines = GetComponentsInChildren<NotepadLine>();
        interactable.activated.AddListener(activateEvent);
        penGrabbable.activated.AddListener(activateEvent);
        penParent = penGrabbable.gameObject.transform.parent;
    }

    private void resetPenPos(SelectExitEventArgs arg0 = null)
    {
        penGrabbable.gameObject.transform.parent = penParent;
        penGrabbable.gameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        penGrabbable.gameObject.transform.localScale = Vector3.one;
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
        resetPenPos();
        penGrabbable.gameObject.SetActive(false);
    }
    protected void PickedUp(SelectEnterEventArgs args = null)
    {
        held = true;
        penGrabbable.gameObject.SetActive(true);
        resetPenPos();
    }
    public void SetLines(NotepadLineElement[] elements)
    {
        lineEvents.Clear();
        clearNotepad();
        TopToBottomLines(elements);
    }

    void TopToBottomLines(NotepadLineElement[] elements)
    {
        int i = 0;
        foreach (var item in elements)
        {
            int lineIndex = i;
            NotepadLine line = lines[lineIndex];
            lineEvents.Add(line, item.lineEvent);
            line.tmp.SetText(item.lineText);
            Debug.Log(item.lineText);
            line.available = item.selectable;
            i++;
        }
    }
    void CenterAlignedLines(NotepadLineElement[] elements)
    {
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
            line.available = item.selectable;
            i++;
        }
    }
    protected void Update()
    {
        if (!held || !penGrabbable.isSelected) { if (currentSelection) { currentSelection.circle.enabled = true; currentSelection = null; } return; }
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
