using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BinocularGrabbable : MonoBehaviour
{
    XRGrabInteractable interactable;

    public GameObject BinocularObjects;
    public GameObject zoomObjects;

    Camera mainCam;
    public Camera binocCamera;

    public Collider headChecker;

    bool binocularEnabled = false;
    bool held = false;
    public float zoom = 5;
    // Start is called before the first frame update
    void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.selectEntered.AddListener(PickedUp);
        interactable.selectExited.AddListener(LetGo);
        mainCam = Camera.main;
        BinocularObjects.transform.SetParent(null);
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
        BinocularObjects.SetActive(true);
    }

    void disableBinocs()
    {
        binocularEnabled = false;
        mainCam.enabled = true;
        BinocularObjects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        WhileHeld();
    }

    void OnDestroy()
    {
        disableBinocs();
    }
}
