using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneSwitcherCube : MonoBehaviour
{
    XRHandedGrabInteractable interactable;

    void Start()
    {
        interactable = gameObject.GetComponent<XRHandedGrabInteractable>();
        interactable.activated.AddListener(nextScene);
    }

    private void nextScene(ActivateEventArgs arg0)
    {
        ScenarioManager.ActiveScenario.videoFinishedScenario.startScene();
    }
}
