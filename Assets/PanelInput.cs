using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInput : MonoBehaviour
{

    public List<GameObject> panels = new List<GameObject>();
    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        setActivePanel(index);
    }

    public void Next()
    {
        if (index < panels.Count)
        {
            index++;
            setActivePanel(index);
        }
    }

    public void Back()
    {
        if (index > 0)
        {
            index--;
            setActivePanel(index);
        }
    }

    void setActivePanel(int panelIndex)
    {
        int i = 0;
        foreach (GameObject panel in panels)
        {
            panel.SetActive(i == panelIndex);
            i++;
        }
    }

    public void Click()
    {
        Debug.Log("CLICK");
    }
}
