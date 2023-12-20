using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NotepadLineElement : MonoBehaviour
{
    public string lineText = "";
    public UnityEvent lineEvent = new UnityEvent();

    void Start()
    {
        lineEvent.AddListener(test);
    }

    private void test()
    {
        Debug.Log(lineText);
    }
}