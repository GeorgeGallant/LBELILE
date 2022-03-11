using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;




[CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler : InputHandler
{
    public InputHelpers.Button button = InputHelpers.Button.None;

    public delegate void StateChange(XRController controller, InputHelpers.Button button);
    public event StateChange OnButtonDown;
    public event StateChange OnButtonUp;

    private bool lastPressed = false;

    public override void HandleState(XRController controller)
    {
        
       if(controller.inputDevice.IsPressed(button, out bool pressed, controller.axisToPressThreshold))
        {
            if(lastPressed != pressed)
            {
                lastPressed = pressed;

                if(pressed)
                {
                    OnButtonDown?.Invoke(controller, button);
                    //Debug.Log("Button Down");
                } else
                {
                    OnButtonUp?.Invoke(controller, button);
                    //Debug.Log("Button Up");
                }
            }
        }
    }
}
