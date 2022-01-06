using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{

    public List<ButtonHandler> ButtonHandlers = new List<ButtonHandler>();

    private XRController controller = null;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleButtonEvents();
    }

    private void HandleButtonEvents()
    {
        foreach(ButtonHandler button in ButtonHandlers)
        {
            button.HandleState(controller);
        }
    }
}
