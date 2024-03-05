using System.Collections;
using System.Collections.Generic;
using ThirdParty;
using UnityEngine;

public class VoiceInteractionTester : MonoBehaviour
{
    ValueWrapper<bool> pressed = new ValueWrapper<bool>(false);
    public KeyCode keyCode = KeyCode.V;
    // Update is called once per frame
    async void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            pressed.Value = true;
            await AzureVoice.Listener(pressed, "tester", ScenarioManager.ActiveScenario.name);
        }
        if (Input.GetKeyUp(keyCode))
        {
            pressed.Value = false;
        }
    }
}
