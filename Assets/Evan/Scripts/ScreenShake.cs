using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Desired duration of the shake effect
    private static float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    private static float shakeMagnitude = 0.25f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition +=  Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.unscaledDeltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public static void TriggerShake(float magmitude)
    {
        shakeMagnitude = magmitude;
        shakeDuration = 0.2f;
    }
}
