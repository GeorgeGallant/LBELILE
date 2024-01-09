using System.Collections.Generic;
using ThirdParty;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PassiveListenerActivatable : BaseSceneActivatable
{
    public string activateIntent;
    public override void activate()
    {
        base.activate();
        OnEnable();
    }
    public override void deactivate()
    {
        base.deactivate();
        OnDisable();
    }
    async void OnEnable()
    {
        if (!sceneActive) return;
        Debug.Log("now passive listening");
        AzureVoice.intentEvent.AddListener(intentListener);
        await AzureVoice.ListenUntil();
    }

    void OnDisable()
    {
        AzureVoice.intentEvent.RemoveListener(intentListener);
        Debug.Log("no longer passive listening");

    }

    private void intentListener((Dictionary<string, AzureVoice.Intent> intents, string topIntent, string initiator) o)
    {
        if (!sceneActive) return;
        if (o.initiator == "once")
        {
            if (o.topIntent.ToLower() == activateIntent.ToLower())
                activateNextScene();
            else
                OnDisable();
        }

    }
}