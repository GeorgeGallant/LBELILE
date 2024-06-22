using System.Collections.Generic;
using ThirdParty;
using UnityEngine;
using UnityEngine.Events;

public class BaseIntentActivatable : BaseActivatable
{
    public IntentEvents[] intents;
    public int attemptsAllowed = 0;
    int attempts = 0;
    public BaseScene badAttemptScene;
    public bool ignoreNoSpeech = true;
    bool listenerEnabled = false;

    protected virtual void OnEnable()
    {
        if (listenerEnabled || Time.time < 1) return;
        listenerEnabled = true;
        foreach (var item in intents)
        {
            Debug.Log($"Length: {item.intents.Length} | Required: {item.requiredAmount}");
            if (item.intents.Length < item.requiredAmount)
            {
                Debug.LogWarning("More intents required than there are intents!");
            }
            foreach (var intent in item.intents)
            {
                if (item.activateScene != null)
                    AzureVoice.intentDestinations.Add(intent, item.activateScene.gameObject.name);
                else Debug.LogWarning($"{item.name} has no activate scene!");
            }
        }
    }

    protected virtual void OnDisable()
    {
        listenerEnabled = false;
        foreach (var item in intents)
        {
            foreach (var intent in item.intents)
            {
                AzureVoice.intentDestinations.Remove(intent);
            }
        }
    }

    protected void badAttempt(string attempt)
    {
        if (!badAttemptScene || (ignoreNoSpeech && attempt == "No speech")) return;
        if (attempts >= attemptsAllowed) badAttemptScene.startScene();
        else attempts++;

    }
}

[System.Serializable]
public class IntentEvents
{
    public string name = "";
    public string[] intents = new string[1];
    public BaseScene activateScene;
    public UnityEvent intentEvent;
    public int requiredAmount = 0;
    private List<string> usedIntents = new List<string>();
    public (bool hadIntent, BaseScene activateScene, bool needMoreIntents) checkIntents(string intent)
    {
        foreach (var item in intents)
        {
            if (item.Trim().ToLower() == intent.ToLower())
            {
                if (requiredAmount > 1 && usedIntents.Count < requiredAmount && !usedIntents.Contains(intent))
                {
                    usedIntents.Add(intent);
                    if (usedIntents.Count < requiredAmount)
                        return (true, null, true);
                }
                else if (requiredAmount > 1 && usedIntents.Contains(intent))
                {
                    return (false, null, true);
                }
                intentEvent.Invoke();
                return (true, activateScene, false);
            }
        }
        return (false, null, false);
    }
}