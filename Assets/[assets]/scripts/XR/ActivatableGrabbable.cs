using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivatableGrabbable : MonoBehaviour
{
    XRGrabInteractable interactable;
    [SerializeField]
    private string handPoseName;
    private string defaultHandPoseName;
    [SerializeField]
    private string objectPoseName;
    [SerializeField]
    private string defaultObjectPoseName;

    private GrabbablePose pose;

    // Start is called before the first frame update
    void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.activated.AddListener(Activated);
        interactable.deactivated.AddListener(Deactivate);
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
        pose = gameObject.GetComponent<GrabbablePose>();

    }

    protected virtual void LetGo(SelectExitEventArgs args = null)
    {

    }

    protected virtual void PickedUp(SelectEnterEventArgs args = null)
    {
        if (pose)
        {
            defaultHandPoseName = pose.poseName;
        }
    }

    protected virtual void Deactivate(DeactivateEventArgs args = null)
    {
        if (handPoseName != string.Empty)
        {
            if (args?.interactorObject != null)
            {
                Animator animator = args.interactorObject.transform.gameObject.GetComponentInChildren<Animator>();

                animator.Play(defaultHandPoseName);
            }
        }
        if (objectPoseName != string.Empty)
        {
            if (args?.interactorObject != null)
            {
                Animator animator = gameObject.GetComponentInChildren<Animator>();

                animator.Play(defaultObjectPoseName);
            }
        }
    }

    protected virtual void Activated(ActivateEventArgs args = null)
    {
        if (handPoseName != string.Empty)
        {
            if (args?.interactorObject != null)
            {
                Animator animator = args.interactorObject.transform.gameObject.GetComponentInChildren<Animator>();

                animator.Play(handPoseName);
            }
        }
        if (objectPoseName != string.Empty)
        {
            if (args?.interactorObject != null)
            {
                Animator animator = gameObject.GetComponentInChildren<Animator>();

                animator.Play(objectPoseName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
