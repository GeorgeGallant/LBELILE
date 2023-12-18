using UnityEngine;
using UnityEngine.Events;

public class NotepadLine : MonoBehaviour
{
    public string lineText = "";
    public UnityEvent lineEvent = new UnityEvent();
}