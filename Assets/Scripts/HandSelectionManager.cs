using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class HandSelectionManager : MonoBehaviour
{

    public string gender;
    public string handColor;

    static BodyType bodyType = BodyType.Masculine;
    static Color? bodyColor = null;

    public Gradient skinGradient = new Gradient();

    public GameObject leftController;
    public GameObject rightController;

    public GameObject maleLeftHandModel;
    public GameObject maleRightHandModel;

    public GameObject femaleLeftHandModel;
    public GameObject femaleRightHandModel;

    List<GameObject> femaleHands;
    List<GameObject> maleHands;

    private ConfigManager config;

    public bool changeLeft = true;
    public bool changeRight = true;

    public void Start()
    {
        config = ConfigManager.Instance;
        var maleRight = Instantiate(maleRightHandModel, rightController.transform, worldPositionStays: false);
        var maleLeft = Instantiate(maleLeftHandModel, leftController.transform, worldPositionStays: false);

        var femaleRight = Instantiate(femaleRightHandModel, rightController.transform, worldPositionStays: false);
        var femaleLeft = Instantiate(femaleLeftHandModel, leftController.transform, worldPositionStays: false);

        maleHands = new List<GameObject> { maleRight, maleLeft };
        femaleHands = new List<GameObject> { femaleLeft, femaleRight };

        if (handColor == null) setHandColor(0);
        setHandGender(bodyType);


    }

    public void setHandColor(float gradientPosition)
    {
        Color newColor = skinGradient.Evaluate(gradientPosition);
        bodyColor = newColor;
        Shader.SetGlobalColor("_HandColor", newColor);
    }

    public void setHandGender(BodyType newBodyType)
    {
        femaleHands.ForEach(x => x.SetActive(false));
        maleHands.ForEach(x => x.SetActive(false));

        switch (newBodyType)
        {
            case BodyType.Masculine:
                maleHands.ForEach(x => x.SetActive(true));
                break;
            case BodyType.Feminine:
                femaleHands.ForEach(x => x.SetActive(true));
                break;
        }

    }


}
public enum BodyType
{
    Masculine,
    Feminine,
    Gloves
}
