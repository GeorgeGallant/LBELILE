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
        else if (intents.Length > 0)
        {
            bool foundScene = false;
            foreach (var item in intents)
            {
                var check = item.checkIntents(o.topIntent);
                if (check.hadIntent)
                {
                    if (check.needMoreIntents)
                    {
                        Debug.Log("Need more intents");
                        continue;
                    }
                    check.activateScene.startScene();
                    foundScene = true;
                    break;
                }
                else if (check.needMoreIntents)
                {
                    Debug.Log("Intent recognized but already used");
                }
            }
            if (foundScene) return;
            badAttempt(o.topIntent);
        }
        else badAttempt(o.topIntent);

    }
}
