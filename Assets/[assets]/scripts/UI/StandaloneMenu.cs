using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StandaloneMenu : MonoBehaviour
{

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void PromptMenu()
    {
        gameObject.SetActive(true);
        gameObject.transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        gameObject.transform.position = Camera.main.transform.position;
    }
}
