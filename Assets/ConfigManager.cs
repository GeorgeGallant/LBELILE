using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance;
    public string gender;
    public string color;


    // called zero
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            Permission.RequestUserPermission(Permission.Microphone);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    } 

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void goToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);


        HandSelectionManager[] hands = FindObjectsOfType<HandSelectionManager>();

        Debug.Log("HAND SELECTION MANAGERS FOUND: " + hands.Length);

        if (hands.Length > 0)
        {
            if (gender != "" && color != "")
            {
                hands[0].setHands(gender, color);
            }
        }

    }

    // called when the game is terminated
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}