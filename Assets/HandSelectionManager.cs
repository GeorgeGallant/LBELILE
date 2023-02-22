using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class HandSelectionManager : MonoBehaviour
{

    public string gender;
    public string handColor;

    private Color dark = new Color32(0x6A, 0x4F, 0x30, 0xFF);
    private Color medium = new Color32(0x94, 0x71, 0x49, 0xFF);
    private Color light = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

    public GameObject leftController;
    public GameObject rightController;

    public GameObject maleLeftHandModel;
    public GameObject maleRightHandModel;

    public GameObject femaleLeftHandModel;
    public GameObject femaleRightHandModel;

    public ConfigManager config;

    public bool changeLeft = true;
    public bool changeRight = true;

    public void Start()
    {
        config = ConfigManager.Instance;
    }

    public void setHands(string gender, string color)
    {
        handColor = color;
        setGender(gender);
        
    }
    public void setGender(string selectedGender)
    {
        gender = selectedGender;

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

            if (left.model != null)
            {
                Destroy(left.model.gameObject);
            }
            Destroy(left.model);
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

            if (right.model != null)
            {
                Destroy(right.model.gameObject);
            }
            Destroy(right.model);
            right.model = newModelR;
        }

        setColor(handColor);

        if (config)
        {
            config.gender = gender;
        }

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

        if (config)
        {
            config.color = selectedColor;
        }

    }

}
