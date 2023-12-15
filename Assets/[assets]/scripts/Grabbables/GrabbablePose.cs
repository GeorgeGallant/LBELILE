using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbablePose : MonoBehaviour
{
    XRGrabInteractable interactable;
    public string poseName;
    // Start is called before the first frame update
    void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
    }

    protected virtual void PickedUp(SelectEnterEventArgs args = null)
    {
        if (args?.interactorObject == null) return;

        Animator animator = args.interactorObject.transform.gameObject.GetComponentInChildren<Animator>(false));
        if (animator == null) return;

        animator.SetBool("grabbing", true);
        animator.Play(poseName);
    }

    protected virtual void LetGo(SelectExitEventArgs args = null)
    {
        if (args?.interactorObject == null) return;

        Animator animator = args.interactorObject.transform.gameObject.GetComponentInChildren<Animator>(false);
        if (animator == null) return;

        animator.SetBool("grabbing", false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
