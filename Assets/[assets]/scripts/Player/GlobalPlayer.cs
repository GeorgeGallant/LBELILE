using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobalPlayer : MonoBehaviour
{
    public static GlobalPlayer instance;
    public Transform playerOffset;
    bool offsetSet = false;
    public static XRDirectInteractor globalLeftController
    {
        get
        {
            return instance.leftController;
        }
    }
    public XRDirectInteractor leftController;
    public static XRDirectInteractor globalRightController
    {
        get
        {
            return instance.rightController;
        }
    }
    public XRDirectInteractor rightController;

    public GameObject[] rays;
    static bool raysEnabled = true;
    static List<MonoBehaviour> rayUsers = new();

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        UpdateRayState();

    }

    public static XRDirectInteractor GetOtherHand(XRDirectInteractor hand)
    {
        if (hand == globalRightController) return globalLeftController;
        else return globalRightController;
    }

    public static void AddRayUser(MonoBehaviour user)
    {
        if (!rayUsers.Contains(user))
            rayUsers.Add(user);
        UpdateRayState();
    }
    public static void RemoveRayUser(MonoBehaviour user)
    {
        if (rayUsers.Contains(user))
            rayUsers.Remove(user);
        UpdateRayState();
    }
    static void UpdateRayState()
    {
        if (raysEnabled && rayUsers.Count == 0)
        {
            foreach (var item in instance.rays)
            {
                if (!item) continue;
                item.SetActive(false);
                raysEnabled = false;
            }
        }
        else if (!raysEnabled && rayUsers.Count > 0)
        {
            foreach (var item in instance.rays)
            {
                if (!item) continue;
                item.SetActive(true);
                raysEnabled = true;
            }
        }
    }
    void Update()
    {
        if (!offsetSet && playerOffset && Camera.main.transform.position.x != 0)
        {
            playerOffset.localPosition = new Vector3(-Camera.main.transform.position.x, 0, -Camera.main.transform.position.z);
            offsetSet = true;
        }
    }
}
