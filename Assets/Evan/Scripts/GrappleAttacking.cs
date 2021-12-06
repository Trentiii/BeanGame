using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GrappleAttacking : MonoBehaviour
{
    //--Editable varibles--
    public GameObject template;

    //--Public varibles--
    [HideInInspector] public bool eating;
    [HideInInspector] public GameObject clone;

    //--Private varibles--
    bool pulling = false;

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
        cloneHolder = transform.GetChild(3).gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If collided with enemy
        if (collision.gameObject.layer == 11)
        {
            startEating(collision.gameObject);
        }
    }

    public void startEating(GameObject enemy)
    {
        eating = true;
        gg.setAttackPoint(enemy.transform.position);
        StartCoroutine(doEating(enemy));
    }

    private void Update()
    {
        if (pulling)
        {
            clone.transform.position = new Vector2(Mathf.Lerp(clone.transform.position.x, transform.position.x, Time.deltaTime * 30), Mathf.Lerp(clone.transform.position.y, transform.position.y, Time.deltaTime * 30));
            gg.setAttackPoint(clone.transform.position);
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

        ConstraintSource self = new ConstraintSource();
        self.sourceTransform = transform;

        pulling = true;
        SpriteRenderer cSR = clone.GetComponent<SpriteRenderer>();

        while (clone.transform.localScale.x > 0.05f)
        {
            if ((clone.transform.position - transform.position).magnitude < 10.04f)
            {
                clone.transform.localScale -= new Vector3(0.05f, 0.05f, 0);

                cSR.color -= new Color(0.06f, 0.06f, 0.06f);
                cSR.color = new Color(Mathf.Clamp(cSR.color.r, 0, 1), Mathf.Clamp(cSR.color.g, 0, 1), Mathf.Clamp(cSR.color.b, 0, 1));

                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        gr.grappleEnded = true;
        pulling = false;
        Destroy(clone);
    }
}
