using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class Remotescript : MonoBehaviour
{
    public string nextSceneName;
    public Vector3 prefPos; 
    public Vector3 prefRotation;
    public float timer;
    public bool useTimer;

    public ButtonHandler buttonPress;

    public void OnDestroy()
    {
        buttonPress.OnButtonDown -= Clicked;
    }

    private void Start()
    {
        this.transform.localPosition = prefPos;
        this.transform.localRotation = Quaternion.Euler(prefRotation);
        if (!useTimer) {
            goodToPress(); 
        }
      
    }
    private void Update()
    {
        if (useTimer) { 
        timer = timer - Time.deltaTime;
            if (timer <= 0)
            {
            goodToPress();
            useTimer = false;
            }
        }
    }
    //a methode to cue once the player is good to press the button
    public void goodToPress()
    {
        buttonPress.OnButtonDown += Clicked;
    }
   
    private void Clicked(XRController controller)
    {
        //de attach from hand so it does not go the next scene
        this.gameObject.transform.SetParent(null);
        //Load next scene
        SceneManager.LoadScene(nextSceneName);
        //testing puposes
        //Debug.Log("click");
    }
}
