using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrossChanger : MonoBehaviour
{
    public static bool grossed;
    public bool title = false;

    public Sprite grossBackground;
    public Sprite grossTitle;

    // Start is called before the first frame update
    void Start()
    {
        if (grossed)
        {
            MenuMusicWaver.mGrossed = true;
            CreditMusicGross.cGrossed = true;
        }

        if (grossed && !title)
        {
            GetComponent<Image>().sprite = grossBackground;
        }
        else if (grossed && title)
        {
            GetComponent<Image>().sprite = grossTitle;
        }
    }
}
