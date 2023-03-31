using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameOver : MonoBehaviour
{

    [TextArea]
    public string Message = "";

    public TextMeshProUGUI Content;

    public string ReplayDestination = "Opening";
    public string RestartDestination = null;
    public ButtonHandler buttonPress;
    public bool buttonPressed;
    public GameObject[] showOnEnable;

    void Awake()
    {
        Content.text = Message;
        if(RestartDestination == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            RestartDestination = scene.name;
        }
        BindXRControllerEvents();
        gameObject.SetActive(buttonPressed);
        foreach (GameObject go in showOnEnable)
        {
            go.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        UnbindXRControllerEvents();
    }

    public void BindXRControllerEvents()
    {
        Debug.Log("bind");
        buttonPress.OnButtonDown += ButtonDown;

    }

    public void UnbindXRControllerEvents()
    {
        if (buttonPress)
        {
            buttonPress.OnButtonDown -= ButtonDown;
        }
    }

    public void ButtonDown(XRController controller, InputHelpers.Button button)
    {
        buttonPressed = !buttonPressed;
        gameObject.SetActive(buttonPressed);
        foreach(GameObject go in showOnEnable)
        {
            go.SetActive(buttonPress);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Load scene " + ReplayDestination);
        ConfigManager.Instance.goToScene(RestartDestination);
    }

    public void RestartScene()
    {
        Debug.Log("Restart scene " + RestartDestination);
        ConfigManager.Instance.goToScene(RestartDestination);
    }


}
