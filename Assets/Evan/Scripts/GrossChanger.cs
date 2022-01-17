using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrossChanger : MonoBehaviour
{
    public static bool grossed;

    public Sprite grossBackground;

    // Start is called before the first frame update
    void Start()
    {
        if (grossed)
        {
            GetComponent<Image>().sprite = grossBackground;
        }
    }
}
