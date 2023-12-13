using System;
using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

public class ScenarioRadioActivatable : BaseScenarioActivatable
{
    public string activateIntent;
    protected override void Start()
    {
        base.Start();
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    private void intentListener((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
    {
        if (scenarioActive) return;

    }
}
