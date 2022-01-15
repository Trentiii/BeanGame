using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMaker : MonoBehaviour
{

    float startPos;
    TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();

        startPos = transform.position.y;
        StartCoroutine(move());
    }

    private IEnumerator move()
    {
        while (transform.position.y < startPos + 1f)
        {
            transform.position += new Vector3(0, 0.01f, 0);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(4);

        GradientAlphaKey aKey = tr.colorGradient.alphaKeys[1];
        while (aKey.alpha > 0)
        {
            aKey = tr.colorGradient.alphaKeys[1];

            var copyGradient = tr.colorGradient;
            var alphaArray = copyGradient.alphaKeys;
            alphaArray[1].alpha = aKey.alpha - 0.01f;
            copyGradient.alphaKeys = alphaArray;
            tr.colorGradient = copyGradient;

            yield return new WaitForEndOfFrame();
        }
    }
}
