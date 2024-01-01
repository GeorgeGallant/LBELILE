using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class ColliderActivatable : BaseSceneActivatable
{
    XRSimpleInteractable interactable;
    protected override void Start()
    {
        base.Start();
        interactable = gameObject.AddComponent<XRSimpleInteractable>();
        interactable.interactionLayers = InteractionLayerMask.NameToLayer("RayInteractibles");
        interactable.colliders.Concat(gameObject.GetComponents<Collider>());
    }
}
