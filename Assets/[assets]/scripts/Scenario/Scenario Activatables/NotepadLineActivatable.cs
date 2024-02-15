using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NotepadLineActivatable : BaseSceneActivatable
{
    NotepadLineElement[] lines;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
}
