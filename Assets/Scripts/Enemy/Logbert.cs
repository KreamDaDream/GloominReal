using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logbert : MonoBehaviour, IEnemy
{
    public Vector3 boundCenter { get; set; }
    public Vector3 bounds { get; set; }

    public int MaxHealth { get; set; }
    public bool dead = false;
    public int health { get; set; }
    private Rigidbody2D rb;
    public ParticleSystem hurtPars { get; set; }
    private int worth;
    private ProjPool logPool;
    private ProjPool coinPool;

    private GameObject coinImage;

    private GameObject plr;

    private bool attacking;
    
    private float lastAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        logPool = GameObject.Find("Pools").transform.Find("Log").GetComponent<ProjPool>();
        coinPool = GameObject.Find("Pools").transform.Find("Coin").GetComponent<ProjPool>();
        MaxHealth = 10;
        worth = 20;
        health = MaxHealth;
        hurtPars = transform.Find("hurtPars").GetComponent<ParticleSystem>();
        coinImage = GameObject.Find("BattleScreen").transform.Find("PointOfCoin").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }

        if (rb.linearVelocity.x > 0)
        {
            GetComponent<Animator>().SetBool("Left", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("Left", true);

        }

        

        if (Time.time - lastAttack > 2 && !attacking && !dead && !plr.GetComponent<plrMovement>().dying)
        {
            attacking = true;
            int ran = Random.Range(1, 3);
            if (ran == 1)
            {
                StartCoroutine(upDowns());
            }
            else if (ran == 2)
            {
                StartCoroutine(steamroll());


            }
        }
    }

    IEnumerator steamroll()
    {
        for (int i = 0; i < 10; i++)
        {
            if (plr.GetComponent<plrMovement>().dying || dead)
            {
                break;
            }

            StartCoroutine(summon());
            StartCoroutine(summon());
            StartCoroutine(summon());

            yield return new WaitForSeconds(0.5f);
        }
        lastAttack = Time.time;
        attacking = false;
    }


    IEnumerator upDowns()
    {
        Vector3[] points = { boundCenter + new Vector3(-bounds.x/2, bounds.y/2, 0), boundCenter + new Vector3(-bounds.x / 4, -bounds.y / 2, 0), boundCenter + new Vector3(0, bounds.y / 2, 0), boundCenter + new Vector3(bounds.x / 4, -bounds.y / 2, 0), boundCenter + new Vector3(bounds.x / 2, bounds.y / 2, 0) };
        GetComponent<OuchBox>().active = true;
        int ran = Random.Range(2, 5);
        bool rev = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        for (int j = 0; j <= ran; j++)
        {
            if (rev)
            {
                rev = false;
                for (int i = 0; i < points.Length; i++)
                {
                    rb.linearVelocity = (points[i] - transform.position).normalized * 15;
                    float ticky = Time.time;

                    while (Time.time - ticky < 0.8f && (points[i] - transform.position).magnitude > 0.2f)
                    {
                        yield return new WaitForEndOfFrame();

                    }

                }
            } else
            {
                rev = true;
                for (int i = points.Length - 1; i >= 0; i--)
                {
                    rb.linearVelocity = (points[i] - transform.position).normalized * 15;
                    float ticky = Time.time;

                    while (Time.time - ticky < 0.8f && (points[i] - transform.position).magnitude > 0.2f)
                    {
                        yield return new WaitForEndOfFrame();

                    }

                }
            }
            
        }

        rb.linearVelocity = (boundCenter - transform.position).normalized * 5;
        float ticky2 = Time.time;

        while (Time.time - ticky2 < 0.8f && (boundCenter - transform.position).magnitude > 0.2f)
        {
            yield return new WaitForEndOfFrame();

        }
        rb.linearVelocity = Vector2.zero;
        GetComponent<BoxCollider2D>().isTrigger = false;

        GetComponent<OuchBox>().active = false;

        lastAttack = Time.time;
        attacking = false;
    }

    IEnumerator summon()
    {
        Vector3 location = boundCenter + new Vector3(Random.Range(-bounds.x / 2, bounds.x / 2), (bounds.y / 2) + 1, Random.Range(-bounds.z / 2, bounds.z / 2));
        GameObject spik = logPool.getObj();
        if (spik != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.summon);
            spik.SetActive(true);
            spik.transform.position = location;
            spik.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0, -5, 0);
            yield return new WaitForSeconds(2f);
            spik.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0, 0, 0);
            spik.SetActive(false);
        }
    }
    public void damage(int amt)
    {

        health = Mathf.Clamp(health - amt, 0, MaxHealth);
        if (health == 0 && !dead)
        {
            StartCoroutine(die());
        }
        hurtPars.Emit(20);
    }

    public IEnumerator die()
    {
        dead = true;
        foreach (SpriteRenderer t in GetComponentsInChildren<SpriteRenderer>())
        {


            t.enabled = false;

        }

        foreach (Transform po in logPool.transform)
        {

            if (po.gameObject.activeSelf)
            {
                po.gameObject.SetActive(false);
            }

        }
        IHan.instance.titleCard = true;

        float angleStep = 360f / worth;
        List<GameObject> coins = new List<GameObject>();
        for (int i = 0; i < worth; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            GameObject coin = coinPool.getObj();
            if (coin != null)
            {
                coin.SetActive(true);
                coin.transform.position = transform.position;
                coin.GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Impulse);
                coin.GetComponent<coinDissa>().dissPoint = coinImage;
                coins.Add(coin);
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < coins.Count; i++)
        {
            yield return new WaitForSeconds(1 / coins.Count);
            coins[i].GetComponent<Rigidbody2D>().linearDamping = 0;

            coins[i].GetComponent<Rigidbody2D>().linearVelocity = (coinImage.transform.position - coins[i].transform.position).normalized * 10f;

        }
        yield return new WaitForSeconds(2f);

        yield return GameObject.Find("BattleScreen").GetComponent<BattleBox>().toggleBox(false, gameObject);
        GameObject.Find("BattleScreen").SetActive(false);
        IHan.instance.titleCard = false;

        Destroy(gameObject);
    }
}

