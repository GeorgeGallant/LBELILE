using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NotepadGrabbable : GrabbablePose
{
    public XRHandedGrabInteractable penGrabbable;
    protected override void Start()
    {
        base.Start();
        penGrabbable.gameObject.SetActive(false);
    }

    protected override void LetGo(SelectExitEventArgs args = null)
    {
        base.LetGo(args);
        penGrabbable.forceHeld = false;
        penGrabbable.gameObject.SetActive(false);
        var otherHand = GlobalPlayer.GetOtherHand(args.interactorObject as XRDirectInteractor);
        args.manager.SelectExit(otherHand as IXRSelectInteractor, penGrabbable);


    }
    protected override void PickedUp(SelectEnterEventArgs args = null)
    {
        base.PickedUp(args);
        penGrabbable.gameObject.SetActive(true);
        penGrabbable.forceHeld = true;
        var otherHand = GlobalPlayer.GetOtherHand(args.interactorObject as XRDirectInteractor);
        args.manager.SelectEnter(otherHand as IXRSelectInteractor, penGrabbable);
    }
}
