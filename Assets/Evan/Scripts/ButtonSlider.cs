using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSlider : MonoBehaviour
{
    Camera mCamera;
    RectTransform rt;

    Vector2 defaultPos;
    Vector2 slidPos;
    Vector2 wantedPos;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();

        defaultPos = rt.anchoredPosition;
        wantedPos = defaultPos;
        slidPos = new Vector3(-104, -57, 0);
        mCamera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        if (mCamera.ScreenToWorldPoint(Input.mousePosition).x < -3.5f)
        {
            wantedPos = defaultPos;
        }
        else
        {
            wantedPos = slidPos;
        }

        //Hi Eric
        rt.anchoredPosition += new Vector2(wantedPos.x/10, 0);
        rt.anchoredPosition = new Vector2(Mathf.Clamp(rt.anchoredPosition.x, -104, 104), -57);
    }
}
