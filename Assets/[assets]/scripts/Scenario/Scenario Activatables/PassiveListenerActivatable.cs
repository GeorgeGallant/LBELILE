using System;
using System.Collections.Generic;
using ThirdParty;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PassiveListenerActivatable : BaseIntentActivatable
{

    ValueWrapper<bool> activeListen = new ValueWrapper<bool>(false);
    public override void activateModifiers()
    {
        OnEnable();
    }
    public override void deactivateModifiers()
    {
        OnDisable();
    }
    protected async override void OnEnable()
    {
        if (!sceneActive) return;
        base.OnEnable();
        Debug.Log("now passive listening");
        AzureVoice.intentEvent.AddListener(intentListener);
        activeListen.Value = true;
        await AzureVoice.Listener(activeListen, "passive", activatableOwner.gameObject.name);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AzureVoice.intentEvent.RemoveListener(intentListener);
        if (activeListen.Value)
            Debug.Log("no longer passive listening");
        activeListen.Value = false;

    }

    private void intentListener((string topIntent, string initiator, string scene) o)
    {
        if (!sceneActive) return;
        if (o.scene != activatableOwner.gameObject.name) return;
        if (o.initiator == "passive")
        {
            var intent = o.topIntent;
            Debug.Log(intent);
            if (intent != string.Empty && intent.ToLower() == activateIntent.ToLower())
            {
                OnDisable();
                activateNextScene();
            }
            else if (intents.Length > 0)
            {
                foreach (var item in intents)
                {
                    var check = item.checkIntents(o.topIntent);
                    if (check.hadIntent)
                    {
                        if (check.activateScene)
                        {
                            OnDisable();
                            check.activateScene.startScene();
                            break;
                        }
                    }
                }
                badAttempt(o.topIntent);
            }
            else badAttempt(o.topIntent);
            // else if (gameObject.activeInHierarchy) OnEnable();
        }

    }
}