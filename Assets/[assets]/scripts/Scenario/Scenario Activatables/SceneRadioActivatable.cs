using System;
using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

public class SceneRadioActivatable : BaseSceneActivatable
{
    public string activateIntent;
    public override void activate()
    {
        base.activate();
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    public override void deactivate()
    {
        base.deactivate();
        AzureVoice.intentEvent.RemoveListener(intentListener);
    }

    private void intentListener((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
    {
        if (scenarioActive) return;
        if (o.initiator == "radio" && o.topIntent.ToLower() == activateIntent.ToLower())
        {
            activateNextScenario();
        }

    }
}
