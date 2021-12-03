using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAttacking : MonoBehaviour
{
    //--Editable varibles--
    public GameObject template;

    //--Public varibles--
    public bool eating;

    //--Private varibles--
    GameObject clone;

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
            Debug.LogError("No CloneHolder found in scene, please add/enable an empty gameobject named \"CloneHolder\"");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collided with enemy
        if (collision.gameObject.layer == 11)
        {
            GameObject enemy = collision.gameObject;

            eating = true;
            gg.setAttackPoint(enemy.transform.position);
            StartCoroutine(doEating(enemy));
        }
    }

    private IEnumerator doEating(GameObject enemy)
    {
        bool spawned = false;
        clone = null;

        //Extend tounge to enemy
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

        while ((clone.transform.position - transform.position).magnitude > 0.1f)
        {
            clone.transform.position = new Vector3(Mathf.Lerp(clone.transform.position.x, transform.position.x, Time.deltaTime * 40), Mathf.Lerp(clone.transform.position.y, transform.position.y, Time.deltaTime * 40), 0);
             gg.setAttackPoint(clone.transform.position);
            yield return new WaitForEndOfFrame();

            Debug.Log((clone.transform.position - transform.position).magnitude > 0.1f);
        }
    }
}
