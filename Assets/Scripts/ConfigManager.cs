using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance;
    public string gender;
    public string color;

    public static string SUBSCRIPTION_KEY = "c23410a601b74a1782766c68ef3f44f7";
    public static string REGION_NAME = "canadacentral";
    public static string APP_ID = "70c9a26e-877c-4d94-a0a0-ff5197d4a2e9";
  //public static string APP_ID = "4ffcac1c-b2fc-48ba-bd6d-b69d9942995a";
    public static string PREDICTION_KEY = "3180862a6aa54c06b16d91bf18279daa";
  public static string LANGUAGE_RESOURCE_KEY = "8fa7fc6de0b742d98bd4490cc56e3c78";

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
        // SceneManager.sceneLoaded += OnSceneLoaded;
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
                // hands[0].setHands(gender, color);
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