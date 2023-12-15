using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelectorUI : MonoBehaviour
{
    public HandSelectionManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<HandSelectionManager>();
    }

    public void setSkinColor(float time)
    {
        manager.setHandColor(time);
    }

    public void setBodyType(BodyType bodyType)
    {
        manager.setHandGender(bodyType);
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
}
