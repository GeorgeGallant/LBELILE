using UnityEngine;
using UnityEngine.Events;

public class BaseIntentActivatable : BaseSceneActivatable
{
    public IntentEvents[] intents;
    public string activateIntent;
    public int attemptsAllowed = 0;
    int attempts = 0;
    public BaseScene badAttemptScene;
    public bool ignoreNoSpeech = true;

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