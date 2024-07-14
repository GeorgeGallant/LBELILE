using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.Interaction.Toolkit;

public class GlobalPlayer : MonoBehaviour
{
    public static GlobalPlayer instance;
    public static bool debugMode = false;
    public static bool heartRateEnabled
    {
        get { return HeartRateEnabled; }
        set
        {
            HeartRateEnabled = value;
            instance.heartRateText.gameObject.SetActive(HeartRateEnabled);
        }
    }
    private static bool HeartRateEnabled = false;
    public bool forceDebug = false;
    public TextMeshPro debugText;
    public TextMeshPro heartRateText;
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
    public static VRInputAsset Controls
    {
        get
        {
            return instance.controls;
        }
    }

    VRInputAsset controls;

    public GameObject[] rays;
    public Teleporter[] teleporters;
    static bool raysEnabled
    {
        get
        {
            if (!instance) return false;
            return instance.RaysEnabled;
        }
        set
        {
            instance.RaysEnabled = value;
        }
    }
    protected bool RaysEnabled = true;
    static bool teleportEnabled = false;
    static List<MonoBehaviour> rayUsers = new();
    static List<TeleportActivatable> teleportUsers = new();
    public static TeleportActivatable[] TeleportUsers;

    private void Awake()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                    {
                        Permission.RequestUserPermission(Permission.Microphone);
                    }));
                }
            }
        }
        controls = new VRInputAsset();
        controls.Enable();
    }

    private void OnPermissionGranted(string obj)
    {
        Debug.LogWarning($"OnPermissionGranted: {obj}");
    }

    private void OnPermissionDeniedAndDontAskAgain(string obj)
    {
        Debug.LogWarning($"OnPermissionDeniedAndDontAskAgain: {obj}");
    }

    private void OnPermissionDenied(string obj)
    {
        Debug.LogWarning($"OnPermissionDenied: {obj}");
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        UpdateRayState();
        UpdateTeleportState();
        instance.debugText.gameObject.SetActive(false);
        instance.heartRateText.gameObject.SetActive(HeartRateEnabled);

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
        if (!instance) return;
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
    public static void AddTeleportUser(TeleportActivatable user)
    {
        if (!teleportUsers.Contains(user))
            teleportUsers.Add(user);
        UpdateTeleportState();
    }
    public static void RemoveTeleportUser(TeleportActivatable user)
    {
        if (teleportUsers.Contains(user))
            teleportUsers.Remove(user);
        UpdateTeleportState();
    }
    static void UpdateTeleportState()
    {
        if (teleportEnabled && teleportUsers.Count == 0)
        {
            foreach (var item in instance.teleporters)
            {
                if (!item) continue;
                item.setDisable();
                teleportEnabled = false;
            }
        }
        else if (!raysEnabled && teleportUsers.Count > 0)
        {
            foreach (var item in instance.teleporters)
            {
                if (!item) continue;
                item.setEnable();
                teleportEnabled = true;
                TeleportUsers = teleportUsers.ToArray();
            }
        }
    }
    public static void ReceiveIntent(string intent)
    {
        if (debugMode || instance.forceDebug)
        {
            instance.debugText.gameObject.SetActive(true);
            instance.debugText.SetText(intent);
        }
    }
    public static void RecieveHeartRate(int heartRate)
    {
        heartRateEnabled = true;
        instance.heartRateText.SetText($"Heart Rate: {heartRate}");

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
