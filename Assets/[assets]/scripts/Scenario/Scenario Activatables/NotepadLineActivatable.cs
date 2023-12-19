using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadLineActivatable : BaseSceneActivatable
{
    public NotepadLineElement[] lines;
    // Start is called before the first frame update
    void Start()
    {
        lines = gameObject.GetComponentsInChildren<NotepadLineElement>(false);
    }

    public override void activate()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
