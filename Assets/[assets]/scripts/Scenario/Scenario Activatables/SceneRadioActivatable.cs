using System;
using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

public class SceneRadioActivatable : BaseSceneActivatable
{
    public string activateIntent;
    private void Start()
    {
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    private void intentListener((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
    {
        if (scenarioActive) return;
        if (o.topIntent.ToLower() == activateIntent.ToLower())
        {
            activateNextScenario();
        }

    }
}