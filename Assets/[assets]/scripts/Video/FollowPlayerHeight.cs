using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerHeight : MonoBehaviour
{
    Transform playerCam;
    void Start()
    {
        playerCam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(0, playerCam.transform.position.y, 0);
    }
}
