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
        interactable.colliders.ForEach((collider) =>
        {
            collider.enabled = false;
        });
    }

    private void selected(ActivateEventArgs arg0)
    {
        activateNextScene();
    }
    public override void activateModifiers()
    {
        GlobalPlayer.AddRayUser(this);
        interactable.colliders.ForEach((collider) =>
        {
            collider.enabled = true;
        });
    }

    public override void deactivateModifiers()
    {
        GlobalPlayer.RemoveRayUser(this);
        interactable.colliders.ForEach((collider) =>
        {
            collider.enabled = false;
        });
    }
}
