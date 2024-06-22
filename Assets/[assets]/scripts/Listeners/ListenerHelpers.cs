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

    protected virtual void OnEnable()
    {
        foreach (var item in intents)
        {
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
    [Header("Optional")]
    public string name = "";
    public string[] intents = new string[1];
    public BaseScene activateScene;
    public UnityEvent intentEvent;
    int amount = 0;
    public int requiredAmount = 0;
    public (bool hadIntent, BaseScene activateScene) checkIntents(string intent)
    {
        foreach (var item in intents)
        {
            if (item.Trim().ToLower() == intent.ToLower())
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