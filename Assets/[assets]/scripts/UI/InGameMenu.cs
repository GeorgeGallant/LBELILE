using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InGameMenu : MonoBehaviour
{
    public ButtonHandler menuButton;
    public GameObject menuGO;

    void Start()
    {
        menuGO.SetActive(false);
        menuButton.OnButtonDown += ToggleMenuState;
    }

    public void ChangeMenuState(bool newState)
    {
        menuGO.SetActive(newState);
        if (newState)
        {
            menuGO.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
            menuGO.transform.position = Camera.main.transform.position;
        }
    }
    void ToggleMenuState(XRController controller, InputHelpers.Button button)
    {
        ChangeMenuState(!menuGO.activeInHierarchy);
    }
}
