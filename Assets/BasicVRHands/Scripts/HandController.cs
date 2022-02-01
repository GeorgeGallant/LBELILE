using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class HandController : MonoBehaviour
{

    private Animator animator;

    public ButtonHandler buttonPress;
    public string controllerName;
    public bool down;
    public List<UnityEvent> DownActions = new List<UnityEvent>();
    public List<UnityEvent> UpActions = new List<UnityEvent>();

    void Start()
    {
        animator = GetComponent<Animator>();
        buttonPress.OnButtonDown += OnDown;
        buttonPress.OnButtonUp += OnUp;
    }
    private void OnDestroy()
    {
        buttonPress.OnButtonDown -= OnDown;
        buttonPress.OnButtonUp -= OnUp;
    }

    private void OnDown(XRController controller, InputHelpers.Button button)
    {
        if (controller.name == controllerName)
        {
            foreach(UnityEvent unityEvent in DownActions)
            {
                unityEvent.Invoke();
            }
            down = true;
        }

    }

    private void OnUp(XRController controller, InputHelpers.Button button)
    {
        if (controller.name == controllerName)
        {
            foreach (UnityEvent unityEvent in UpActions)
            {
                unityEvent.Invoke();
            }
            down = false;
        }
    }

}
