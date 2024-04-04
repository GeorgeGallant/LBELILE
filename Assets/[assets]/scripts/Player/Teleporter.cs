using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    public enum Hand { Left, Right }
    public Hand hand;
    bool teleportActive;
    LineRenderer line;
    Material lineMat;
    Vector3 checkPoint;
    TeleportActivatable teleportCandidate;

    public Color availableColor = Color.blue;
    public Color unavailableColor = Color.red;

    bool thumbstickInput = false;

    TeleportActivatable[] users;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponentInChildren<LineRenderer>();
        lineMat = new Material(line.material);
        line.material = lineMat;
    }

    public void setEnable()
    {
        Debug.Log("Teleporter enabled");
        if (hand == Hand.Left)
            GlobalPlayer.Controls.VRInput.LeftThumbstick.performed += thumbstickMovement;
        if (hand == Hand.Right)
            GlobalPlayer.Controls.VRInput.RightThumbstick.performed += thumbstickMovement;
    }

    public void setDisable()
    {
        if (hand == Hand.Left)
            GlobalPlayer.Controls.VRInput.LeftThumbstick.performed -= thumbstickMovement;
        if (hand == Hand.Right)
            GlobalPlayer.Controls.VRInput.RightThumbstick.performed -= thumbstickMovement;
        teleportActive = false;
        line.enabled = false;
    }

    private void thumbstickMovement(InputAction.CallbackContext context)
    {
        thumbstickInput = true;
        Vector2 thumbstick = context.ReadValue<Vector2>();
        Debug.Log(thumbstick);
        bool frameActive = Vector2.Distance(Vector2.down, thumbstick) < 0.3f;
        if (teleportActive && !frameActive && teleportCandidate) teleportCandidate.Selected();
        line.enabled = frameActive;
        teleportActive = frameActive;
    }

    void teleportUpdate()
    {
        drawRay();
        users = GlobalPlayer.TeleportUsers;
        float distance = 5000;
        TeleportActivatable candidate = null;
        foreach (var item in users)
        {
            var itemDistance = Vector3.Distance(item.transform.position, checkPoint);
            if (itemDistance < distance && itemDistance < 1f)
            {
                distance = itemDistance;
                candidate = item;
            }
        }
        if (candidate != teleportCandidate) Debug.Log(teleportCandidate);
        teleportCandidate = candidate;
        lineMat.SetColor("_BaseColor", teleportCandidate ? availableColor : unavailableColor);
    }

    void drawRay()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 velocity = transform.forward * 7;
        Vector3 point = transform.position;
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        float delta = 0.01f;
        while (point.y > GlobalPlayer.instance.transform.position.y)
        {
            points.Add(point);
            point += velocity * delta;
            velocity += gravity * delta;
        }
        checkPoint = point;
        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());

    }

    // Update is called once per frame
    void Update()
    {
        if (thumbstickInput && teleportActive) { teleportUpdate(); return; }
        line.enabled = false;
        thumbstickInput = false;
    }
}
