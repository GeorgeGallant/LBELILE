using System.Collections.Generic;

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
        BaseScene scene = null;
        for (int i = 0; i < ScenarioManager.ActiveKeywords.Count; i++)
        {
            if (keySceneDict.TryGetValue(ScenarioManager.ActiveKeywords[i], out scene))
            {
                scene.startScene();
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