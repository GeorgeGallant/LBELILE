using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager instance;
    public BaseScene firstScenario;
    public KeyObject[] gameObjectDictionaryCreator = new KeyObject[0];
    public static Dictionary<ScenarioObject, GameObject> gameObjectDictionary = new Dictionary<ScenarioObject, GameObject>();
    public BaseScene activeScenario;

    public static BaseScene ActiveScenario
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
        firstScenario.startScene();
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
                if (keys[i].spawnPosition) go.transform.SetPositionAndRotation(keys[i].spawnPosition.position, keys[i].spawnPosition.rotation);
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
        foreach (var objectKey in gameObjectDictionary.Keys)
        {
            if (objects.Contains(objectKey))
                gameObjectDictionary[objectKey].SetActive(true);
            else
            {
                gameObjectDictionary[objectKey].SetActive(false);
            }
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
    public Transform spawnPosition;
}

public enum ScenarioObject
{
    Radio,
    Binoculars,
    Notepad,
    Remote
}