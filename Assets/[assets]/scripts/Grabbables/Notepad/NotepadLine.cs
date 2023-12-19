using TMPro;
using UnityEngine;

public class NotepadLine : MonoBehaviour
{
    public Collider bounds;
    public TextMeshPro tmp;
    public SpriteRenderer circle;
    public bool available = true;

    public void clear()
    {
        tmp.SetText("");
        available = false;
    }
    public void setText(string newText)
    {
        tmp.SetText(newText);
        available = true;
    }

}