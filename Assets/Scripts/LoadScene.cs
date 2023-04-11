using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void goToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
