using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneCam : MonoBehaviour
{
    public Transform myTransform;
    public int nextSlide = 1;
    public GameObject[] Points;
    bool resume = true;
    float interval = 0;
    private float x;
    private float y;

    bool transitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = Points[0].transform.position + new Vector3(0, 0, -10);
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.y;
        
    }

    // Update is called once per frame
    void Update()
    {
        //float x2 = Points[nextSlide].transform.position.x;
        //float y2 = Points[nextSlide].transform.position.y;

        //x = Mathf.Lerp(x, x2, .1f);
        //y = Mathf.Lerp(y, y2, .1f);

        //Debug.Log(Points[nextSlide].transform.position);

        //StartCoroutine(Transition());
        if (Input.GetMouseButtonDown(0))
        {
            if(nextSlide != 3)
            nextSlide++;
            print(nextSlide);
            transitioning = true;
        }

        if (transitioning)
        {
            Vector2 change = Vector2.Lerp(gameObject.transform.position, Points[nextSlide].transform.position, 0.01f);
            transform.position = new Vector3(change.x, change.y, transform.position.z);

            if(Vector2.Distance(transform.position, Points[nextSlide].transform.position) < 0.01f)
            {
                transitioning = false;
            }
        }

        if (gameObject.transform.position == Points[nextSlide].transform.position + new Vector3(0,0,-10))
        {
            Debug.Log("slide++");
        }
        //gameObject.transform.position = new Vector3(x, y, -10);
    }

    
}
