using UnityEngine;
using System.Collections;
using UnityEngine.LowLevelPhysics2D;
using NUnit.Framework;
using System.Collections.Generic;
public class Orb : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float wanderRange;
    [SerializeField] private GameObject battleBox;

    [SerializeField] private float range;
    
    private GameObject plr;
    private Rigidbody2D rb;
    private Vector3 ogPoint;
    private bool attacking;
    private float lastMove = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GameObject.Find("PLAYER");
        rb = GetComponent<Rigidbody2D>();
        ogPoint = transform.position;
        battleBox = plr.GetComponent<plrMovement>().box;
    }

    // Update is called once per frame
    void Update()
    {
        if ((plr.transform.position - transform.position).magnitude <= range)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                rb.linearVelocity = (plr.transform.position - transform.position).normalized * speed;
            }
        } else
        {
            StartCoroutine(patrol());
        }
        if (!attacking && (plr.transform.position - transform.position).magnitude < 1.5f && plr.GetComponent<plrMovement>().challenged == false)
        {
            StartCoroutine(challenge());
        }
        
    }

    IEnumerator patrol()
    {
        rb.linearVelocity = Vector2.zero;

        if (Time.time - lastMove > 2)
        {
            lastMove = Time.time;
            Vector3 randomPoint = ogPoint + new Vector3(Random.Range(-wanderRange, wanderRange), Random.Range(-wanderRange, wanderRange), 0);
            float start = Time.time;
            while ((transform.position - randomPoint).magnitude > 0.1 && (Time.time - start) < 1.5 && (plr.transform.position - transform.position).magnitude > range)
            {
                rb.linearVelocity = (randomPoint - transform.position).normalized * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        rb.linearVelocity = Vector2.zero;
        yield return null;
    }

    IEnumerator challenge()
    {
        if (plrMovement.instance.canBeEngaged)
        {
            attacking = true;
            plr.GetComponent<plrMovement>().canMove = false;
            plr.GetComponent<plrMovement>().canInteract = false;
            plr.GetComponent<plrMovement>().canOpenMenu = false;
            plrMovement.instance.canBeEngaged = false;

            plr.GetComponent<plrMovement>().menu.SetActive(false);

            plr.GetComponent<plrMovement>().challenged = true;
            rb.bodyType = RigidbodyType2D.Static;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.notice);
            AudioManager.Instance.ChangeMusic(null);

            GetComponent<SpriteRenderer>().sortingOrder = 6;
            yield return new WaitForSeconds(1);
            battleBox.SetActive(true);
            StartCoroutine(battleBox.GetComponent<BattleBox>().toggleBox(true, gameObject));
        }
    }
}
