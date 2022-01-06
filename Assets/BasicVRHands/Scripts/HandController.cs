using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class HandController : MonoBehaviour {

    private Animator animator;

	public ButtonHandler buttonPress;
    public string controllerName;
	public bool down;

	void Start () {
        animator = GetComponent<Animator>();
		buttonPress.OnButtonDown += OnDown;
		buttonPress.OnButtonUp += OnUp;
	}
    private void OnDestroy()
    {
        buttonPress.OnButtonDown -= OnDown;
        buttonPress.OnButtonUp -= OnUp;
    }

    private void OnDown(XRController controller)
    {
        if(controller.name == controllerName)
        {
            Debug.Log(controller.name+"DOWN");
            down = true;
        }
        
    }

    private void OnUp(XRController controller)
    {
        if (controller.name == controllerName)
        {
            Debug.Log(controller.name + "UP");
            down = false;
        }
    }

    void Update () {
        animator.SetBool("isGrabbing", down);
	}
}
