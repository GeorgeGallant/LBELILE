using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandedGrabInteractable : XRGrabInteractable
{
    [SerializeField]
    private Transform LeftHandAttachTransform;
    [SerializeField]
    private Transform RightHandAttachTransform;

    private Transform m_OriginalAttachTransform;

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

    //  OnSelectEntering - set attachTransform - then call base
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        if (isSelected) return;
        if (args.interactorObject.Equals(GlobalPlayer.globalLeftController))
        {
            Debug.Log($"Left hand");
            attachTransform.SetPositionAndRotation(LeftHandAttachTransform.position, LeftHandAttachTransform.rotation);
        }
        else if (args.interactorObject.Equals(GlobalPlayer.globalRightController))
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
}
