using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobalPlayer : MonoBehaviour
{
    public static GlobalPlayer instance;
    public static XRDirectInteractor globalLeftController
    {
        get
        {
            return instance.leftController;
        }
    }
    public XRDirectInteractor leftController;
    public static XRDirectInteractor globalRightController
    {
        get
        {
            return instance.rightController;
        }
    }
    public XRDirectInteractor rightController;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

}
