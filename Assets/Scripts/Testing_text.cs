using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Testing_text : MonoBehaviour
{
    public string FileName;
    private string KeyFilePath;
    // Start is called before the first frame update
    void Start()
    {
        pullKeyAndId();
    }
    private void pullKeyAndId()
    {
        KeyFilePath = Application.dataPath + "/" + FileName;
        string[] textfile = File.ReadAllLines(KeyFilePath);
        Debug.Log(textfile[1]);
        Debug.Log(textfile[3]);
        Debug.Log(textfile[5]);
        Debug.Log(textfile[7]);

    }
}
