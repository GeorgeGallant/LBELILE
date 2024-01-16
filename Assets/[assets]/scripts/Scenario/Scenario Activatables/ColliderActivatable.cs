using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class ColliderActivatable : BaseSceneActivatable
{
    XRSimpleInteractable interactable;

    protected override void StartSetup()
    {
        interactable = gameObject.AddComponent<XRSimpleInteractable>();
        interactable.interactionLayers = InteractionLayerMask.NameToLayer("RayInteractibles");
        interactable.colliders.Concat(gameObject.GetComponents<Collider>());
        interactable.activated.AddListener(selected);
    }

    private void selected(ActivateEventArgs arg0)
    {
        activateNextScene();
    }
    public override void activate()
    {
        base.activate();
        GlobalPlayer.AddRayUser(this);
    }

    public override void deactivate()
    {
        base.deactivate();
        GlobalPlayer.RemoveRayUser(this);
    }
}
