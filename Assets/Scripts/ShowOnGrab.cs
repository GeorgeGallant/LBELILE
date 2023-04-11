using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShowOnGrab : MonoBehaviour
{
    // Start is called before the first frame update

    private Color dark = new Color32(0x6A, 0x4F, 0x30, 0xFF);
    private Color medium = new Color32(0x94, 0x71, 0x49, 0xFF);
    private Color light = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

    public GameObject maleHand;
    public GameObject femaleHand;
    public GameObject subMaleHand;
    public GameObject subFemaleHand;
    public GameObject[] hideOnGrab;
    GameObject selected;
    public GameObject controller;

    private void Awake()
    {
        maleHand.SetActive(false);
        femaleHand.SetActive(false);
        Debug.Log(ConfigManager.Instance.gender);
        Material material;
        switch (ConfigManager.Instance.gender) {
            case "male":
                selected = maleHand;
                material = subMaleHand.GetComponent<Renderer>().material;
                setMaterial(material);
                break;

            case "female":
                selected = femaleHand;
                material = subFemaleHand.GetComponent<Renderer>().material;
                setMaterial(material);
                break;
            default:
                selected = maleHand;
                material = subMaleHand.GetComponent<Renderer>().material;
                setMaterial(material);
                break;
        }

        

    }

    private Material setMaterial(Material material)
    {
        switch (ConfigManager.Instance.color)
        {
            case "dark":
                material.color = dark;
                break;
            case "medium":
                material.color = medium;
                break;
            case "light":
                material.color = light;
                break;
        }

        return material;
    }

    public void Grab(SelectEnterEventArgs press)
    { 
        if(press.interactor.name != controller.name) return;
        foreach(GameObject go in hideOnGrab)
        {
            go.SetActive(false);
        }
        controller.GetComponent<XRController>().model.gameObject.SetActive(false);
        selected.SetActive(true);
    }

    public void LetGo(SelectExitEventArgs press)
    {
        if (press.interactor.name != controller.name) return;
        foreach (GameObject go in hideOnGrab)
        {
            go.SetActive(true);
        }
        controller.GetComponent<XRController>().model.gameObject.SetActive(true);
        selected.SetActive(false);
    }

    public void Pressed()
    {
        Debug.Log("PRESS");
        Animator animator= selected.GetComponent<Animator>();
        animator.SetBool("closed", true);
    }
    public void Released()
    {
        Debug.Log("RELEASE");
        Animator animator = selected.GetComponent<Animator>();
        animator.SetBool("closed", false);
    }

}
