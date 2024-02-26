using System.Collections.Generic;
using ThirdParty;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PassiveListenerActivatable : BaseSceneActivatable
{
    public IntentEvents[] intents;
    public string activateIntent;
    ValueWrapper<bool> activeListen = new ValueWrapper<bool>(false);
    public override void activateModifiers()
    {
        OnEnable();
    }
    public override void deactivateModifiers()
    {
        OnDisable();
    }
    async void OnEnable()
    {
        if (!sceneActive) return;
        Debug.Log("now passive listening");
        AzureVoice.intentEvent.AddListener(intentListener);
        activeListen.Value = true;
        await AzureVoice.Listener(activeListen, "passive");
    }

    void OnDisable()
    {
        AzureVoice.intentEvent.RemoveListener(intentListener);
        Debug.Log("no longer passive listening");
        activeListen.Value = false;

    }

    private void intentListener((string topIntent, string initiator) o)
    {
        if (!sceneActive) return;
        if (o.initiator == "once")
        {
            Debug.Log(o.topIntent);
            if (o.topIntent.ToLower() == activateIntent.ToLower())
                activateNextScene();
            else if (gameObject.activeInHierarchy) OnEnable();
        }
        else if (intents.Length > 0)
        {
            foreach (var item in intents)
            {
                var check = item.checkIntents(o.topIntent);
                if (check.hadIntent)
                {
                    check.activateScene.startScene();
                    break;
                }
            }
        }

    }
}