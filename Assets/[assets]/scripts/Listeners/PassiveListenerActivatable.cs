using System;
using System.Collections.Generic;
using ThirdParty;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PassiveListenerActivatable : BaseIntentActivatable
{

    ValueWrapper<bool> activeListen = new ValueWrapper<bool>(false);
    bool listening = false;
    public bool getLastIntentWhenInactive = false;
    public override void activateModifiers()
    {
        activateListener();
    }
    public override void deactivateModifiers()
    {
        deactivateListener();
    }
    async void activateListener()
    {
        if (!sceneActive) return;
        if (activeListen.Value) return;
        base.OnEnable();
        Debug.Log("now passive listening");
        listening = true;
        AzureVoice.intentEvent.AddListener(intentListener);
        activeListen.Value = true;
        await AzureVoice.Listener(activeListen, "passive", activatableOwner.gameObject.name);
    }
    void deactivateListener()
    {
        AzureVoice.intentEvent.RemoveListener(intentListener);
        if (activeListen.Value)
        {
            Debug.Log("no longer passive listening");
            activeListen.Value = false;
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        activateListener();
    }

    protected override void OnDisable()
    {
        if (!getLastIntentWhenInactive) deactivateListener();
        base.OnDisable();


    }

    void OnDestroy()
    {
        deactivateListener();
    }

    private void intentListener((string topIntent, string initiator, string scene) o)
    {
        Debug.Log($"Intent: {o.topIntent}, Scene: {o.scene} equal to {activatableOwner.gameObject.name}?, Initiator: {o.initiator}");
        List<string> intentList = new List<string>();
        if (!sceneActive) return;
        if (o.scene != activatableOwner.gameObject.name) return;
        if (o.initiator == "passive")
        {
            var intent = o.topIntent;
            if (intents.Length > 0)
            {
                bool foundScene = false;
                foreach (var item in intents)
                {
                    foreach (var itemIntent in item.intents)
                    {
                        intentList.Add(itemIntent);
                    }
                    var check = item.checkIntents(intent);
                    if (check.hadIntent)
                    {
                        Debug.Log("Had intent");
                        if (check.needMoreIntents)
                        {
                            Debug.Log("Need more intents");
                            continue;
                        }
                        if (check.activateScene)
                        {
                            Debug.Log("Intent hit");
                            OnDisable();
                            Debug.Log(check.activateScene.gameObject.name);
                            check.activateScene.startScene();
                            foundScene = true;
                            break;
                        }
                    }
                    else if (check.needMoreIntents)
                    {
                        Debug.Log("Intent recognized but already used");
                    }
                }
                if (foundScene) return;
                string[] intentArray = intentList.ToArray();
                Debug.Log($"Could not find intent: {o.topIntent} inside {string.Join(',', intentArray)}");
                badAttempt(o.topIntent);
            }
            else badAttempt(o.topIntent);
            if (!gameObject.activeInHierarchy) deactivateListener();
            // else if (gameObject.activeInHierarchy) OnEnable();
        }

    }
}