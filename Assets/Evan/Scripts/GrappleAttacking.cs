using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAttacking : MonoBehaviour
{
    //--Editable varibles--
    public GameObject template;

    //--Private varibles--
    GameObject enemy;

    //--Private references--
    GrapplingGun gg;
    GrapplingRope gr;
    GameObject cloneHolder;

    // Start is called before the first frame update
    void Start()
    {
        //Gets references
        gg = transform.GetChild(0).GetComponent<GrapplingGun>();
        gr = transform.GetChild(0).GetChild(0).GetComponent<GrapplingRope>();
        cloneHolder = GameObject.Find("CloneHolder");

        //Error check refereces
        if (cloneHolder == null)
        {
            Debug.LogError("No CloneHolder found in scene, please add an empty gameobject named \"CloneHolder\"");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collided with enemy
        if (collision.gameObject.layer == 11)
        {
            enemy = collision.gameObject;

            gg.eating = true;
            gg.setAttackPoint(collision.transform.position);
            StartCoroutine(eating());
        }
    }

    private IEnumerator eating()
    {
        bool spawned = false;
        GameObject clone;

        while (true)
        {
            if (gr.straightLine && !spawned)
            {
                spawned = true;
                clone = Instantiate(template, enemy.transform.position, enemy.transform.localRotation, cloneHolder.transform);
                clone.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<SpriteRenderer>().sprite;
                Destroy(enemy);                
            }

            if (spawned)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
