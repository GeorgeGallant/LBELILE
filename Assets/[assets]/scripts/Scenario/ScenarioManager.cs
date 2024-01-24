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
    public GenericScene activeScenario;
    public List<string> activeKeywords = new List<string>();
    public static List<string> ActiveKeywords
    {
        get
        {
            return instance.activeKeywords;
        }
    }

    public static GenericScene ActiveScenario
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

    public static void enableScenarioObjects(SceneObject[] objects)
    {
        Dictionary<ScenarioObject, Transform> dict = new Dictionary<ScenarioObject, Transform>();
        foreach (var item in objects)
        {
            dict.Add(item.scenarioObject, item.spawnPosition);
        }
        foreach (var objectKey in gameObjectDictionary.Keys)
        {
            if (dict.Keys.Contains(objectKey))
            {
                gameObjectDictionary[objectKey].SetActive(true);
                if (dict[objectKey])
                {
                    gameObjectDictionary[objectKey].transform.SetPositionAndRotation(dict[objectKey].position, dict[objectKey].rotation);
                }
            }
            else
            {
                gameObjectDictionary[objectKey].SetActive(false);
            }
        }
    }

    public void addKeyword(string keyword)
    {
        if (!activeKeywords.Contains(keyword))
        {
            activeKeywords.Add(keyword);
        }
        else
        {
            Debug.Log($"{keyword} already in keywords");
        }
    }

    public void removeKeyword(string keyword)
    {
        if (activeKeywords.Contains(keyword))
        {
            activeKeywords.Remove(keyword);
        }
        else
        {
            Debug.Log($"{keyword} not in keywords");
        }
    }

    [System.Serializable]
    public struct KeyObject
    {
        public ScenarioObject scenarioObject;
        public GameObject gameObject;
    }
}
[System.Serializable]
public struct SceneObject
{
    public ScenarioObject scenarioObject;
    public Transform spawnPosition;
}

public enum ScenarioObject
{
    Radio,
    Binoculars,
    Notepad,
    Remote
}