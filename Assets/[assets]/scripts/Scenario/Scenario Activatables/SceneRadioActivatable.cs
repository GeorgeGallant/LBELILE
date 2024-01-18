using System;
using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

public class SceneRadioActivatable : BaseSceneActivatable
{
    public string activateIntent;
    public override void activateModifiers()
    {
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    public override void deactivateModifiers()
    {
        base.deactivate();
        AzureVoice.intentEvent.RemoveListener(intentListener);
    }

    void OnEnable()
    {
        if (!sceneActive) return;
        activate();
    }

    void OnDisable()
    {
        deactivate();
    }

    private void intentListener((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
    {
        if (!sceneActive) return;
        if (o.initiator == "radio" && o.topIntent.ToLower() == activateIntent.ToLower())
        {
            activateNextScene();
        }

    }
}
