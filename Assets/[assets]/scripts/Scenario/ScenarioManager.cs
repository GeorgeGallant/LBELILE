using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager instance;
    public BaseScenario firstScenario;
    public KeyObject[] gameObjectDictionaryCreator = new KeyObject[0];
    public static Dictionary<ScenarioObject, GameObject> gameObjectDictionary = new Dictionary<ScenarioObject, GameObject>();
    public BaseScenario activeScenario;

    public static BaseScenario ActiveScenario
    {
        get
        {
            return instance.activeScenario;
        }
        set
        {
            instance.activeScenario = value;
        }
    }
    public GameObject videoSphere;
    public static GameObject VideoSphere
    {
        get
        {
            return instance.videoSphere;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        createObjects();
        firstScenario.startScenario();
    }

    static void createObjects()
    {
        GameObject parent = new GameObject("Scenario Objects");
        var keys = instance.gameObjectDictionaryCreator;
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
            if (gameObjectDictionary.ContainsKey(key.scenarioObject))
            {
                Debug.LogError($"{key.scenarioObject} already has an object associated with it! Check for duplicates!");
                continue;
            }
            gameObjectDictionary.Add(key.scenarioObject, go);
            go.transform.SetParent(parent.transform);
        }
    }

    public static void enableScenarioObjects(ScenarioObject[] objects)
    {
        foreach (var go in gameObjectDictionary.Values)
        {
            go.SetActive(false);
        }
        foreach (var objectKey in objects)
        {
            gameObjectDictionary[objectKey].SetActive(true);
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