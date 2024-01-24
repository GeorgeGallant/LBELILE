using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class DynamicSceneSwitcher : BaseScene
{
    public KeyScene[] keyScenes;
    Dictionary<string, BaseScene> keySceneDict = new Dictionary<string, BaseScene>();
    public BaseScene defaultScene;
    private void Start()
    {
        foreach (var item in keyScenes)
        {
            keySceneDict.Add(item.Key, item.scene);
        }
    }
    public override void startScene()
    {
        var keys = keySceneDict.Keys.ToArray();
        for (int i = 0; i < keys.Length; i++)
        {
            if (ScenarioManager.ActiveKeywords.Contains(keys[i]))
            {
                keySceneDict[keys[i]].startScene();
                return;
            }
        }
        if (defaultScene) defaultScene.startScene();
    }
    [System.Serializable]
    public struct KeyScene
    {
        public string Key;
        public BaseScene scene;
    }
}