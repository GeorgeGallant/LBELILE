using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public BaseScenario firstScenario;
    public KeyObject[] gameObjectDictionaryCreator = new KeyObject[0];
    public Dictionary<ScenarioObject, GameObject> gameObjectDictionary = new Dictionary<ScenarioObject, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        createObjects();
    }

    void createObjects()
    {
        var keys = gameObjectDictionaryCreator;
        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            var go = key.gameObject;
            if (!go.activeInHierarchy)
            {
                go = Instantiate(go);
                key.gameObject = go;
            }
            go.SetActive(false);
            gameObjectDictionary.Add(key.scenarioObject, go);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public struct KeyObject
{
    public ScenarioObject scenarioObject;
    public GameObject gameObject;
}

public enum ScenarioObject
{
    Radio,
    Binoculars,
    Notepad,
    Remote
}