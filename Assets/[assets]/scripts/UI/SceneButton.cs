using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneButton : MonoBehaviour
{
    public string scene;
    private void Awake()
    {
        GetComponentInChildren<Button>().onClick.AddListener(GoToScene);
    }
    private void GoToScene()
    {
        SceneManager.LoadScene(scene);
    }
}
