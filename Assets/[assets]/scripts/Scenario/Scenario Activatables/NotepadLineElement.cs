using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NotepadLineElement : MonoBehaviour
{
    public string lineText = "";
    public UnityEvent lineEvent = new UnityEvent();
    public BaseScene videoFinishedScene;

    void Start()
    {
        lineEvent.AddListener(test);
        if (videoFinishedScene)
            lineEvent.AddListener(openScene);
    }

    private void test()
    {
        Debug.Log(lineText);
    }

    private void openScene()
    {
        videoFinishedScene.startScene();
    }
}