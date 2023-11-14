using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobalPlayer : MonoBehaviour
{
    public static XRDirectInteractor globalLeftController;
    public XRDirectInteractor leftController;
    public static XRDirectInteractor globalRightController;
    public XRDirectInteractor rightController;

    private void Start()
    {
        globalLeftController = leftController;
        globalRightController = rightController;
    }


}
