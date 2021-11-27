using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Remotescript : MonoBehaviour
{
    public string nextSceneName;
    public Transform hand;
    public Vector3 prefPos; 
    public Vector3 prefRotation;
    public SteamVR_Action_Boolean TV;
    public SteamVR_Input_Sources handType;

    private void Start()
    {
        hand = GameObject.FindWithTag("Attach").transform;
        //find the player hand and attach
        this.gameObject.transform.SetParent(hand);
        //this is to change the remot so it look correct in the players hand
        this.transform.localPosition = prefPos;
        this.transform.localRotation = Quaternion.Euler(prefRotation);
        //goodToPress();
    }
    //a methode to cue once the player is good to press the button
    public void goodToPress()
    {
        TV.AddOnStateDownListener(Clicked, handType);
    }
   
    private void Clicked(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //de attach from hand so it does not go the next scene
        this.gameObject.transform.SetParent(null);
        //Load next scene
        SceneManager.LoadScene(nextSceneName);
        //testing puposes
        //Debug.Log("click");
    }
}
