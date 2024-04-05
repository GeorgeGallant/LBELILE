using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager instance;
    public BaseScene firstScenario;
    public KeyObject[] gameObjectDictionaryCreator = new KeyObject[0];
    public SoundScapeInitializer[] soundScapeCreator = new SoundScapeInitializer[0];
    public Dictionary<ScenarioObject, GameObject> gameObjectDictionary = new Dictionary<ScenarioObject, GameObject>();
    public Dictionary<string, SoundScape> soundScapeDictionary = new Dictionary<string, SoundScape>();
    public string cluProjectName;
    public string cluDeploymentName;
    public string playingSoundscape;

    public static Dictionary<ScenarioObject, GameObject> GameObjectDictionary
    {
        get
        {
            return instance.gameObjectDictionary;
        }
    }
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
            if (instance.occupier == value)
            {
                instance.occupier = null;
                instance.activeScenario = value;
            }
            else if (instance.occupier == null) instance.activeScenario = value;
        }
    }
    public static bool AttemptOccupy(BaseScene scene)
    {
        if (!instance.occupier)
        {
            instance.occupier = scene;
            return true;
        }
        return false;
    }
    public static List<MonoBehaviour> Blockers
    {
        get { return instance.blockers; }
    }
    public static bool AllowNewScene
    {
        get { return Blockers.Count == 0; }
    }
    public static BaseScene Occupier
    {
        get
        {
            return instance.occupier;
        }
    }
    internal List<MonoBehaviour> blockers = new List<MonoBehaviour>();
    internal BaseScene occupier;
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
        createSoundscapes();
        firstScenario.startScene();
    }

    static (Vector3 pos, Quaternion rot) getSpawn()
    {
        var pos = Camera.main.transform.forward * 0.25f;
        pos.y = Camera.main.transform.position.y;
        Quaternion rot = Quaternion.LookRotation((pos - Camera.main.transform.position).normalized);
        return (pos, rot);
    }
    public static void playSoundscape(string key)
    {
        if (key == instance.playingSoundscape) return;
        var dict = instance.soundScapeDictionary;
        var arr = dict.Values.ToArray();
        if (arr.Length == 0) return;
        foreach (var item in arr)
        {
            item.StopSoundscape();
        }
        key = key.Trim();
        if (key == "") return;
        SoundScape ss;
        var exists = dict.TryGetValue(key, out ss);
        if (exists)
        {
            ss.PlaySoundscape();
        }
        else Debug.LogError($"No soundscape key: {key}");
        instance.playingSoundscape = key;
    }

    void createSoundscapes()
    {
        GameObject parent = new GameObject("Soundscape Objects");
        var keys = soundScapeCreator;
        if (keys.Length == 0) return;
        for (int i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            var go = new GameObject(key.keyName, typeof(SoundScape));
            go.transform.SetParent(parent.transform);
            var audiosource = go.GetComponent<AudioSource>();
            audiosource.clip = key.clip;
            audiosource.loop = true;
            var ss = go.GetComponent<SoundScape>();
            ss.fadeTime = key.fadeTime;
            ss.MaxVolume = key.maxVolume;
            soundScapeDictionary.Add(key.keyName, ss);
        }
    }

    void createObjects()
    {
        GameObject parent = new GameObject("Scenario Objects");
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
            if (GameObjectDictionary.ContainsKey(key.scenarioObject))
            {
                Debug.LogError($"{key.scenarioObject} already has an object associated with it! Check for duplicates!");
                continue;
            }
            GameObjectDictionary.Add(key.scenarioObject, go);
            go.transform.SetParent(parent.transform);
        }
    }

    public static void enableScenarioObjects(SceneObject[] objects)
    {
        Dictionary<ScenarioObject, (Vector3 pos, Quaternion rot, bool use)> dict = new Dictionary<ScenarioObject, (Vector3 pos, Quaternion rot, bool use)>();
        foreach (var item in objects)
        {
            if (item.spawnInFront || item.spawnPosition != null)
            {
                var spawnPoint = getSpawn();
                dict.Add(item.scenarioObject, item.spawnInFront ? (spawnPoint.pos, spawnPoint.rot, true) : (item.spawnPosition.position, item.spawnPosition.rotation, true));
            }
            else
                dict.Add(item.scenarioObject, (Vector3.zero, Quaternion.identity, false));
        }
        foreach (var objectKey in GameObjectDictionary.Keys)
        {
            if (dict.Keys.Contains(objectKey))
            {
                GameObjectDictionary[objectKey].SetActive(true);
                if (dict.ContainsKey(objectKey))
                {
                    if (dict[objectKey].use)
                        GameObjectDictionary[objectKey].transform.SetPositionAndRotation(dict[objectKey].pos, dict[objectKey].rot);
                }
            }
            else
            {
                GameObjectDictionary[objectKey].SetActive(false);
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
    [System.Serializable]
    public class SoundScapeInitializer
    {
        public string keyName = "";
        public AudioClip clip = null;
        public float maxVolume = 0.4f;
        public float fadeTime = 1;
    }
}
[System.Serializable]
public struct SceneObject
{
    public ScenarioObject scenarioObject;
    public Transform spawnPosition;
    public bool spawnInFront;
}

public enum ScenarioObject
{
    Radio,
    Binoculars,
    Notepad,
    Remote
}