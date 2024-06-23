using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugToggle : MonoBehaviour
{
    public Toggle debugToggle;
    public float timeUntilNext = 0;
    // Start is called before the first frame update
    void Start()
    {
        debugToggle.isOn = GlobalPlayer.debugMode;
        debugToggle.onValueChanged.AddListener(debugToggle_onValueChanged);
    }

    private void debugToggle_onValueChanged(bool arg0)
    {
        GlobalPlayer.debugMode = arg0;
        debugToggle.isOn = GlobalPlayer.debugMode;
        debugToggle.interactable = false;
        timeUntilNext = 0.5f;
    }

    private void Update()
    {
        if (timeUntilNext > 0)
        {
            timeUntilNext -= Time.deltaTime;
            if (timeUntilNext <= 0)
            {
                debugToggle.interactable = true;
            }
        }
    }
}
