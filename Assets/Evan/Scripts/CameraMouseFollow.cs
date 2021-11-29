﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseFollow : MonoBehaviour
{
    [Tooltip("Scales how much the camaera will move with the mouse")]
    [SerializeField] float scrollDist = 5;
    [Tooltip("Speed at which the camera moves")]
    [SerializeField] float speed = 9;

    // Update is called once per frame
    void Update()
    {
        //Get current mouse position
        Vector2 currentMousePos = Input.mousePosition;

        //Turn that into a screen postion scaled by scrollDist
        Vector3 wantedPoint = new Vector3(scrollDist * Mathf.Clamp(currentMousePos.x / Screen.width, 0, 1) - (scrollDist / 2), scrollDist * Mathf.Clamp(currentMousePos.y / Screen.height, 0, 1) - (scrollDist / 2), -20);

        //Lerp towards that wanted point at speed
        transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, wantedPoint.x, Time.deltaTime * speed), Mathf.Lerp(transform.localPosition.y, wantedPoint.y, Time.deltaTime * speed), wantedPoint.z);
    }
}