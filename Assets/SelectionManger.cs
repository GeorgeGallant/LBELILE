using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Video;
using UnityEngine.Audio;

public class SelectionManger : MonoBehaviour
{

    public string gender;
    public string handColor;

    public Color dark;
    public Color medium;
    public Color light;

    public GameObject leftController;
    public GameObject rightController;

    public GameObject maleLeftHandModel;
    public GameObject maleRightHandModel;

    public GameObject femaleLeftHandModel;
    public GameObject femaleRightHandModel;

    public VideoPlayer video;
    public List<AudioSource> audio;

    public bool active;

    public bool selectionMade = false;

    public bool changeLeft = true;
    public bool changeRight = true;

    public List<ButtonHandler> ButtonHandlers = new List<ButtonHandler>();

    public void setGender(string selectedGender)
    {
        gender = selectedGender;

        Debug.Log(gender);

        if (changeLeft)
        {
            XRController left = leftController.GetComponent<XRController>();
            Transform newModelL;

            if (gender == "female")
            {
                newModelL = Instantiate(femaleLeftHandModel, leftController.transform).transform;
            }
            else
            {
                newModelL = Instantiate(maleLeftHandModel, leftController.transform).transform;
            }

            Destroy(left.model.gameObject);
            left.model = newModelL;
        }

        if (changeRight)
        {

            XRController right = rightController.GetComponent<XRController>();
            Transform newModelR;

            if (gender == "female")
            {
                newModelR = Instantiate(femaleRightHandModel, rightController.transform).transform;
            }
            else
            {
                newModelR = Instantiate(maleRightHandModel, rightController.transform).transform;
            }

            Destroy(right.model.gameObject);
            right.model = newModelR;
        }

        setColor(handColor);
    }

    public void setColor(string selectedColor)
    {
        handColor = selectedColor;

        if(changeLeft)
        {
            XRController left = leftController.GetComponent<XRController>();
            Material leftMaterial;

            if (gender == "female")
            {
                leftMaterial = left.model.gameObject.transform.Find("female_hand_low_l").gameObject.GetComponent<Renderer>().material;
            }
            else
            {
                leftMaterial = left.model.gameObject.transform.Find("male_hand_low_l").gameObject.GetComponent<Renderer>().material;
            }

            switch (handColor)
            {
                case "dark":
                    leftMaterial.color = dark;
                    break;
                case "medium":
                    leftMaterial.color = medium;
                    break;
                case "light":
                    leftMaterial.color = light;
                    break;
            }
        }

        if(changeRight)
        {

            XRController right = rightController.GetComponent<XRController>();
            Material rightMaterial;

            if (gender == "female")
            {
                rightMaterial = right.model.gameObject.transform.Find("male_hand_high_r").gameObject.GetComponent<Renderer>().material;
            }
            else
            {
                rightMaterial = right.model.gameObject.transform.Find("male_hand_high_r").gameObject.GetComponent<Renderer>().material;
            }

            switch (handColor)
            {
                case "dark":
                    rightMaterial.color = dark;
                    break;
                case "medium":
                    rightMaterial.color = medium;
                    break;
                case "light":
                    rightMaterial.color = light;
                    break;
            }
        }


    }

    public void StartVideo()
    {
        active = false;
        selectionMade = true;
        video.Play();
        foreach(AudioSource sound in audio)
        {
            sound.Play(0);
        }
    }

}
