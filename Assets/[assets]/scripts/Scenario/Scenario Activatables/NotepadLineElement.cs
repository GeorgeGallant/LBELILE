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
        if (videoFinishedScene)
            lineEvent.AddListener(openScene);
    }

    private void openScene()
    {
        videoFinishedScene.startScene();
    }
}