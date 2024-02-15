using UnityEngine;

public class RayActivator : MonoBehaviour
{
    void OnEnable()
    {
        GlobalPlayer.AddRayUser(this);
    }
    void OnDisable()
    {
        GlobalPlayer.RemoveRayUser(this);
    }
}