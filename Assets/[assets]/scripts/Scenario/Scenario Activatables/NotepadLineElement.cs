using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NotepadLineElement : MonoBehaviour
{
    public string lineText = "";
    public UnityEvent lineEvent = new UnityEvent();
    public BaseScene videoFinishedScene;
    public float sizeMultiply = 1;
    public bool bold, italic, strikethrough, underline = false;
    public bool isSelectable
    {
        get
        {
            var keywordRestrict = true;
            if (keywords.Length > 0)
                foreach (var item in keywords)
                {
                    if (behaviour == KeywordBehaviour.Require)
                    {
                        if (
                        !ScenarioManager.ActiveKeywords.Contains(item))
                        {
                            keywordRestrict = false;
                            break;
                        }
                    }
                    if (behaviour == KeywordBehaviour.Restrict)
                    {
                        if (
                        ScenarioManager.ActiveKeywords.Contains(item))
                        {
                            keywordRestrict = false;
                            break;
                        }
                    }
                }
            return selectable && keywordRestrict;
        }
    }
    public bool selectable = true;
    public string[] keywords;
    public KeywordBehaviour behaviour;
    public enum KeywordBehaviour
    {
        Require,
        Restrict
    }

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