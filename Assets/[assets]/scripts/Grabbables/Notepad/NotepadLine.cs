using TMPro;
using UnityEngine;

public class NotepadLine : MonoBehaviour
{
    public Collider bounds;
    public TextMeshPro tmp;
    public SpriteRenderer circle;
    public float lineSize;
    public bool available = true;
    private void Start()
    {
        lineSize = tmp.fontSize;
    }

    public void clear()
    {
        tmp.SetText("");
        available = false;
        circle.enabled = false;
    }
    public void setText(string newText)
    {
        tmp.SetText(newText);
        available = true;
    }
    public void setSize(float sizeMultiply)
    {
        tmp.fontSize = lineSize * sizeMultiply;
    }
    public void setStyle(bool bold, bool italic, bool underline, bool strikeThrough)
    {
        FontStyles styles = FontStyles.Normal;
        if (bold) styles = styles | FontStyles.Bold;
        if (italic) styles = styles | FontStyles.Italic;
        if (underline) styles = styles | FontStyles.Underline;
        if (strikeThrough) styles = styles | FontStyles.Strikethrough;

        tmp.fontStyle = styles;
    }

}