using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BinocularGrabbable : MonoBehaviour
{
    XRHandedGrabInteractable interactable;

    public GameObject BinocularCamera;
    public GameObject zoomObjects;

    Camera mainCam;
    public Camera binocCamera;

    public Collider headChecker;

    bool binocularEnabled = false;
    bool held = false;
    public float zoom = 5;

    public UnityEvent onHeldUp = new UnityEvent();
    public UnityEvent onReleased = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        interactable = gameObject.GetComponent<XRHandedGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
        mainCam = Camera.main;
        BinocularCamera.transform.SetParent(null);
        BinocularCamera.SetActive(false);
        zoomObjects.SetActive(false);

        var activatables = zoomObjects.GetComponentsInChildren<BinocularActivatable>(true);
        foreach (var item in activatables)
        {
            item.SubscribeActivatable(this);
        }
    }

    void PickedUp(SelectEnterEventArgs args = null)
    {
        held = true;
    }

    void LetGo(SelectExitEventArgs args = null)
    {
        held = false;
        if (binocularEnabled) disableBinocs();
    }

    void WhileHeld()
    {
        if (!held) return;

        var heldUp = headChecker.bounds.Contains(mainCam.transform.position) && Vector3.Angle(mainCam.transform.forward, transform.forward) < 30;

        if (heldUp != binocularEnabled)
        {
            if (heldUp) enableBinocs();
            else disableBinocs();
        }
        if (binocularEnabled)
        {
            binocCamera.transform.rotation = mainCam.transform.rotation;
            zoomObjects.transform.position = -binocCamera.transform.forward * zoom;
        }
    }

    void enableBinocs()
    {
        binocularEnabled = true;
        mainCam.enabled = false;
        BinocularCamera.SetActive(true);
        zoomObjects.SetActive(true);
        onHeldUp.Invoke();
    }

    void disableBinocs()
    {
        binocularEnabled = false;
        mainCam.enabled = true;
        BinocularCamera.SetActive(false);
        zoomObjects.SetActive(false);
        onReleased.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        WhileHeld();
    }

    void OnDisable()
    {
        if (mainCam)
            disableBinocs();
    }

    void OnDestroy()
    {
        if (mainCam)
            mainCam.enabled = true;
        onReleased?.Invoke();
        if (BinocularCamera)
            Destroy(BinocularCamera);
        if (zoomObjects)
            Destroy(zoomObjects);
    }
}
