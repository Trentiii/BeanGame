using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseFollow : MonoBehaviour
{
    [Tooltip("Scales how much the camaera will move with the mouse")]
    [SerializeField] float scrollDist = 5;
    [Tooltip("Speed at which the camera moves")]
    public float speed = 9;

    [HideInInspector] public bool centered = false;
    [HideInInspector] public bool dying = false;

    // Update is called once per frame
    void Update()
    {
        //Get current mouse position
        Vector2 currentMousePos = Input.mousePosition;

        Vector3 wantedPoint = new Vector3(0.5f, 0.5f, -20); //Get center as defualt
        if (!centered)
        {
            //Turn that into a screen postion scaled by scrollDist
            wantedPoint = new Vector3(scrollDist * Mathf.Clamp(currentMousePos.x / Screen.width, 0, 1) - (scrollDist / 2), scrollDist * Mathf.Clamp(currentMousePos.y / Screen.height, 0, 1) - (scrollDist / 2), -20);
        }

        if (!dying)
        {
            //Lerp towards that wanted point at speed
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, wantedPoint.x, Time.deltaTime * speed), Mathf.Lerp(transform.localPosition.y, wantedPoint.y, Time.deltaTime * speed), wantedPoint.z);
        }
        else
        {
            //Lerp towards that wanted point at speed (unscaled time)
            transform.localPosition = new Vector3(Mathf.Lerp(transform.localPosition.x, wantedPoint.x, Time.unscaledDeltaTime * speed), Mathf.Lerp(transform.localPosition.y, wantedPoint.y, Time.unscaledDeltaTime * speed), wantedPoint.z);
        }
    }
}
