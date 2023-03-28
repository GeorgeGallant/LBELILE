using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    [TextArea]
    public string Message = "";

    public TextMeshProUGUI Content;

    public string ReplayDestination = "Opening";
    public string RestartDestination = null;

    void Start()
    {
        Content.text = Message;
        if(RestartDestination == null)
        {
            Scene scene = SceneManager.GetActiveScene();
            RestartDestination = scene.name;
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(RestartDestination);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(RestartDestination);
    }


}
