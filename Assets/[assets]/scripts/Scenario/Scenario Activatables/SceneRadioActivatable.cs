using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;
using UnityEngine.Events;

public class SceneRadioActivatable : BaseSceneActivatable
{
    public IntentEvents[] intents;
    public string activateIntent;
    public override void activateModifiers()
    {
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    public override void deactivateModifiers()
    {
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

    private void intentListener((string topIntent, string initiator) o)
    {
        if (!sceneActive) return;
        if (o.initiator == "radio" && o.topIntent.ToLower() == activateIntent.ToLower())
        {
            activateNextScene();
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
[System.Serializable]
public class IntentEvents
{
    [Header("Optional")]
    public string name = "";
    public string[] intents;
    public BaseScene activateScene;
    public UnityEvent intentEvent;
    int amount = 0;
    public int requiredAmount = 0;
    public (bool hadIntent, BaseScene activateScene) checkIntents(string intent)
    {
        foreach (var item in intents)
        {
            if (item.ToLower() == intent.ToLower())
            {
                amount++;
                if (amount < requiredAmount) return (false, null);
                intentEvent.Invoke();
                return (true, activateScene);
            }
        }
        return (false, null);
    }
}