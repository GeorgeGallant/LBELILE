using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntentRecorder : MonoBehaviour
{
    public static IntentRecorder instance;
    List<IntentRecord> record = new List<IntentRecord>();
    static bool warned = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static void RecordIntent((string speech, string intentRecognized, string initiator, string result, string intentDestination) i)
    {
        if (i.intentRecognized.ToLower() == "no speech") return;
        if (!instance)
        {
            if (warned) return;
            Debug.LogWarning("No intent recorder in scene!");
            warned = true;
            return;
        }
        var newRecord = new IntentRecord
        {
            speech = i.speech,
            intentRecognized = i.intentRecognized,
            initiator = i.initiator,
            timestamp = System.DateTime.Now.ToString(),
            scene = ScenarioManager.instance.activeScenario.gameObject.name,
            destination = i.intentDestination,
            jsonOutput = JsonConvert.DeserializeObject(i.result)
        };
        instance.record.Add(newRecord);
        Debug.Log(newRecord);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        if (record.Count == 0) return;
        string jsonOutput = JsonConvert.SerializeObject(new RecordOutput
        {
            scenario = SceneManager.GetActiveScene().name,
            intents = record.ToArray()
        });
        string timestamp = System.DateTime.Now.ToString();
        string pattern = "[\\~#%&*{}/:<>?|\"-]";
        Regex regEx = new Regex(pattern);
        string filename = $"{timestamp} - {SceneManager.GetActiveScene().name}.json";
        string sanitized = Regex.Replace(regEx.Replace(filename, "-"), @"/s+", "-");
        string path = Path.Combine(Application.persistentDataPath, "intentLogs", sanitized);
        var file = new FileInfo(path);
        file.Directory.Create();
        File.WriteAllText(file.FullName, jsonOutput);
        Debug.Log($"Wrote {path}");
    }
    public struct RecordOutput
    {
        public string scenario;
        public IntentRecord[] intents;
    }
    public struct IntentRecord
    {
        public string speech;
        public string intentRecognized;
        public string initiator;
        public string timestamp;
        public string scene;
        public string destination;
        public object jsonOutput;

    }
}
