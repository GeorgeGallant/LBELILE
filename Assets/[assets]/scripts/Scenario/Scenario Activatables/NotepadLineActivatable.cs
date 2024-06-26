using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NotepadLineActivatable : BaseSceneActivatable
{
    NotepadLineElement[] lines;
    public Sprite overlayImage;
    // Start is called before the first frame update

    protected override void StartSetup()
    {
        lines = gameObject.GetComponentsInChildren<NotepadLineElement>(false);
    }

    public override void activateModifiers()
    {
        Debug.Log("activate notepad");
        NotepadGrabbable notepad = ScenarioManager.GameObjectDictionary[ScenarioObject.Notepad].GetComponent<NotepadGrabbable>();
        notepad.SetLines(lines);
        notepad.SetImage(overlayImage);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
