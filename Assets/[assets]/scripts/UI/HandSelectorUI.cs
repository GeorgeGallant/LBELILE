using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandSelectorUI : MonoBehaviour
{
    public HandSelectionManagerV2 manager;
    public Image maleSelector;
    public Image femaleSelector;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<HandSelectionManagerV2>();
        setSelector();
    }

    void setSelector()
    {
        if (HandSelectionManagerV2.bodyType == BodyType.Masculine)
        {
            maleSelector.gameObject.SetActive(true);
            femaleSelector.gameObject.SetActive(false);
        }
        else
        {
            maleSelector.gameObject.SetActive(false);
            femaleSelector.gameObject.SetActive(true);
        }
    }

    public void setSkinColor(float time)
    {
        manager.setHandColor(time);
    }

    public void setBodyType(BodyType bodyType)
    {
        manager.setHandGender(bodyType);
        setSelector();
    }

    public void setBodyToMasculine()
    {
        setBodyType(BodyType.Masculine);
    }
    public void setBodyToFeminine()
    {
        setBodyType(BodyType.Feminine);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        GlobalPlayer.AddRayUser(this);
    }
    void OnDisable()
    {
        GlobalPlayer.RemoveRayUser(this);
    }
}
