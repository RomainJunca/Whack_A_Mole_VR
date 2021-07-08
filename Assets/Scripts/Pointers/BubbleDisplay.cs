﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BubbleDisplay : MonoBehaviour
{
    // The parent we will follow in terms of object position.
    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private GameObject bubbleRender;

    [SerializeField]
    private GameObject controllerRender;

    [SerializeField]
    private bool parentX = true;

    [SerializeField]
    private bool parentY = true;

    [SerializeField]
    private bool parentZ = false;

    [SerializeField]
    private float offsetX = 0f;

    [SerializeField]
    private float offsetY = 0f;

    [SerializeField]
    private float offsetZ = 0f;

    [SerializeField]
    private LaserMapper laserMapper;

    [SerializeField]
    private Image motorSpaceRender;

    [SerializeField]
    private Color motorActiveColor;

    [SerializeField]
    private Color motorDisabledColor;

    private float newPosX;
    private float newPosY;
    private float newPosZ;

    private Vector3 ownPosition;

    [System.Serializable]
    public class EnterMotorSpaceEvent : UnityEvent<bool> {}
    public EnterMotorSpaceEvent enterMotorStateEvent;
    
    private bool render = true;
    // Start is called before the first frame update
    void Awake()
    {
        ownPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Update our world position to be equivalent to the parent, for the axis chosen.
        newPosX = parentX ? parent.transform.position.x : ownPosition.x;
        newPosY = parentY ? parent.transform.position.y : ownPosition.y;
        newPosZ = parentZ ? parent.transform.position.z : ownPosition.z;

        Vector3 newPos = new Vector3(newPosX, newPosY, newPosZ);
        if (laserMapper.CoordinateWithinMotorSpace(newPos)) {
            this.transform.position = new Vector3(newPosX + offsetX, newPosY + offsetY, newPosZ + offsetZ);
            if (!render) {
                render = true;
                bubbleRender.SetActive(true);
                controllerRender.SetActive(false);
                motorSpaceRender.color = motorActiveColor;
                enterMotorStateEvent.Invoke(true);
            }
        } else {
            if (render) {
                render = false;
                bubbleRender.SetActive(false);
                controllerRender.SetActive(true);
                motorSpaceRender.color = motorDisabledColor;
                enterMotorStateEvent.Invoke(false);
            }
        }
    }

    public void UpdateOwnPosition(Vector3 newPosition) {
        ownPosition = newPosition;
    }
}
