using UnityEngine;

public class ComboActivatable : MonoBehaviour
{
    protected ComboActivator comboActivator;
    public void initialize(ComboActivator comboActivatable)
    {
        comboActivator = comboActivatable;
    }
    protected void changeActivationState(bool state)
    {
        if (comboActivator)
            comboActivator.updateCombo(this, state);
    }
}