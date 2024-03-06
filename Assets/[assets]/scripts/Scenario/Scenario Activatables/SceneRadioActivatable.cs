using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;
using UnityEngine.Events;

public class SceneRadioActivatable : BaseIntentActivatable
{

    public override void activateModifiers()
    {
        AzureVoice.intentEvent.AddListener(intentListener);
    }

    public override void deactivateModifiers()
    {
        AzureVoice.intentEvent.RemoveListener(intentListener);
    }

    protected override void OnEnable()
    {
        if (!sceneActive) return;
        base.OnEnable();
        activate();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        deactivate();
    }

    private void intentListener((string topIntent, string initiator, string scene) o)
    {
        if (!sceneActive) return;
        if (o.scene != activatableOwner.gameObject.name) return;
        if (o.initiator == "radio" && o.topIntent.ToLower() == activateIntent.ToLower())
        {
            activateNextScene();
        }
        else if (intents.Length > 0)
        {
            bool foundScene = false;
            foreach (var item in intents)
            {
                var check = item.checkIntents(o.topIntent);
                if (check.hadIntent)
                {
                    check.activateScene.startScene();
                    foundScene = true;
                    break;
                }
            }
            if (foundScene) return;
            badAttempt(o.topIntent);
        }
        else badAttempt(o.topIntent);

    }
}
