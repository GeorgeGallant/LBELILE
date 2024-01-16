using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandedGrabInteractable : XRGrabInteractable
{
    enum Hand
    {
        None,
        Left,
        Right
    }
    [SerializeField]
    private Transform LeftHandAttachTransform;
    [SerializeField]
    private Transform RightHandAttachTransform;

    private Transform m_OriginalAttachTransform;
    [SerializeField]
    Hand restrictGrabbableHand;

    protected override void Awake()
    {
        if (attachTransform == null)
        {
            attachTransform = new GameObject($"{name} attach").transform;
            attachTransform.parent = transform;
        }
        m_OriginalAttachTransform = attachTransform;
        base.Awake();
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if ((interactor as XRDirectInteractor) == GlobalPlayer.globalLeftController && (restrictGrabbableHand == Hand.Right || interactorsSelecting.Contains(GlobalPlayer.globalRightController))) return false;
        if ((interactor as XRDirectInteractor) == GlobalPlayer.globalRightController && (restrictGrabbableHand == Hand.Left || interactorsSelecting.Contains(GlobalPlayer.globalLeftController))) return false;
        return base.IsSelectableBy(interactor);
    }

    //  OnSelectEntering - set attachTransform - then call base
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (args.interactorObject.Equals(GlobalPlayer.globalLeftController) && LeftHandAttachTransform)
        {
            Debug.Log($"Left hand");
            attachTransform.SetPositionAndRotation(LeftHandAttachTransform.position, LeftHandAttachTransform.rotation);
        }
        else if (args.interactorObject.Equals(GlobalPlayer.globalRightController) && RightHandAttachTransform)
        {
            Debug.Log($"Right hand");
            attachTransform.SetPositionAndRotation(RightHandAttachTransform.position, RightHandAttachTransform.rotation);
        }
        else
        {
            // Handle case where interactor is not left hand or right hand (socket?)
            attachTransform.SetPositionAndRotation(m_OriginalAttachTransform.position, m_OriginalAttachTransform.rotation);
        }
        base.OnSelectEntering(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }
}
