using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTargets : MonoBehaviour
{
    public void goToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
