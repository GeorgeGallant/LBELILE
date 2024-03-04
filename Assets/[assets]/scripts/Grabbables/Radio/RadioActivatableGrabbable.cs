using ThirdParty;
using UnityEngine.XR.Interaction.Toolkit;

public class RadioActivatableGrabbable : ActivatableGrabbable
{
    ValueWrapper<bool> pressed = new ValueWrapper<bool>(false);
    protected async override void Activated(ActivateEventArgs args = null)
    {
        base.Activated(args);
        pressed.Value = true;
        await AzureVoice.Listener(pressed, "radio", ScenarioManager.ActiveScenario.gameObject.name);
    }

    protected override void Deactivate(DeactivateEventArgs args = null)
    {
        base.Deactivate(args);
        pressed.Value = false;
    }
}