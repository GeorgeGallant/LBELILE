using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class HandSelectionManagerV2 : MonoBehaviour
{
    public bool forceBodyType = false;
    public BodyType forcedBodyType = BodyType.Masculine;
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

    public void Start()
    {
        config = ConfigManager.Instance;
        if (bodyColor == null) setHandColor(0);
        var maleRight = Instantiate(maleRightHandModel, rightController.transform, worldPositionStays: false);
        var maleLeft = Instantiate(maleLeftHandModel, leftController.transform, worldPositionStays: false);

        var femaleRight = Instantiate(femaleRightHandModel, rightController.transform, worldPositionStays: false);
        var femaleLeft = Instantiate(femaleLeftHandModel, leftController.transform, worldPositionStays: false);

        maleHands = new List<GameObject> { maleRight, maleLeft };
        femaleHands = new List<GameObject> { femaleLeft, femaleRight };

        if (bodyColor == null) setHandColor(0);
        setHandGender(forceBodyType ? forcedBodyType : bodyType);


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
