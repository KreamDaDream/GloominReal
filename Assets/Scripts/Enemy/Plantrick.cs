using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plantrick : MonoBehaviour, IEnemy
{
    public Vector3 boundCenter { get; set; }
    public Vector3 bounds { get; set; }

    public int MaxHealth { get; set; }
    public bool dead = false;
    public int health { get; set; }
    private Rigidbody2D rb;
    public ParticleSystem hurtPars { get; set; }
    private int worth;
    private ProjPool seedPool;
    private ProjPool coinPool;
    [SerializeField] private GameObject physicalShootHole;
    private GameObject coinImage;
    private GameObject plant;

    private GameObject plr;

    private bool sproted = false;
    private bool attacking;

    private float lastAttack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seedPool = GameObject.Find("Pools").transform.Find("Seed").GetComponent<ProjPool>();
        coinPool = GameObject.Find("Pools").transform.Find("Coin").GetComponent<ProjPool>();
        MaxHealth = 10;
        worth = 20;
        health = MaxHealth;
        coinImage = GameObject.Find("BattleScreen").transform.Find("PointOfCoin").gameObject;
        plant = transform.Find("plant").gameObject;
        hurtPars = plant.transform.Find("hurtPars").GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null)
        {
            plr = GameObject.Find("PLAYER");
        }

      



        if (Time.time - lastAttack > 1.5f && !attacking && !dead && !plr.GetComponent<plrMovement>().dying)
        {
            attacking = true;
            int ran = Random.Range(1, 3);
            if (ran == 1)
            {
                StartCoroutine(Burst());

            }
            else if (ran == 2)
            {
                StartCoroutine(seedShoot());



            }
        }
    }

    IEnumerator Burst()
    {
       
        int ran = Random.Range(5, 11);
        for (int i = 0; i <= ran; i++)
        {
            if (plr.GetComponent<plrMovement>().dying || dead)
            {
                break;
            }
            Vector3 location = boundCenter + new Vector3(Random.Range(-bounds.x / 2, bounds.x / 2), Random.Range(-bounds.y / 2, bounds.y / 2), Random.Range(-bounds.z / 2, bounds.z / 2));

            transform.position = location;
            yield return StartCoroutine(comeUp(false, false, true));

            yield return new WaitForSeconds(0.2f);
            yield return comeUp(true, true);
            plant.GetComponent<OuchBox>().active = true;

            yield return new WaitForSeconds(0.2f);
            plant.GetComponent<OuchBox>().active = false;

            yield return comeUp(false, true);

        }

        yield return comeUp(true, true);
        yield return new WaitForSeconds(2f);
        yield return comeUp(false, true);

        lastAttack = Time.time;
        attacking = false;
    }

    IEnumerator comeUp(bool up, bool aggres, bool warning = false)
    {
        if (!warning)
        {
            int sped;
            if (aggres)
            {
                sped = 10;
            }
            else
            {
                sped = 50;

            }

            if (up)
            {
                GetComponent<BoxCollider2D>().enabled = true;
                for (float i = 0; i < sped; i++)
                {
                    plant.transform.localPosition = Vector3.Lerp(new Vector3(0, -1.2f, 0), new Vector3(0, 1f, 0), i / sped);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                for (float i = 0; i < sped; i++)
                {

                    plant.transform.localPosition = Vector3.Lerp(new Vector3(0, 1f, 0), new Vector3(0, -1.2f, 0), i / sped);
                    yield return new WaitForSeconds(0.01f);

                }
                GetComponent<BoxCollider2D>().enabled = false;

            }
        } else
        {
            for (float i = 0; i < 25; i++)
            {
                plant.transform.localPosition = Vector3.Lerp(new Vector3(0, -1.2f, 0), new Vector3(0, 1f, 0), i / 50);
                yield return new WaitForSeconds(0.005f);
            }
        
            for (float i = 25; i >= 0; i--)
            {

                plant.transform.localPosition = Vector3.Lerp(new Vector3(0, -1.2f, 0), new Vector3(0, 1f, 0), i / 50);
                yield return new WaitForSeconds(0.005f);

            }
        }
    }
    IEnumerator seedShoot()
    {
        if (!sproted)
        {
            plant.GetComponent<Animator>().SetBool("sprouting", true);
            yield return new WaitForSeconds(0.5f);
            plant.GetComponent<Animator>().SetBool("sprouted", true);
            plant.GetComponent<Animator>().SetBool("sprouting", false);
            yield return comeUp(false, true);


        }
        int ran = Random.Range(2, 5);
        for (int i = 0; i <= ran; i++)
        {
            if (plr.GetComponent<plrMovement>().dying || dead)
            {
                break;
            }
            Vector3 location = boundCenter + new Vector3(Random.Range(-bounds.x / 2, bounds.x / 2), Random.Range(-bounds.y / 2, bounds.y / 2), Random.Range(-bounds.z / 2, bounds.z / 2));

            transform.position = location;
            yield return comeUp(true, true);

            for (int j = 0; j < 3; j++)
            {
                StartCoroutine(summon());
                yield return new WaitForSeconds(0.5f);

            }
            yield return new WaitForSeconds(0.5f);
            yield return comeUp(false, true);
        }

        yield return new WaitForSeconds(2f);
        plant.GetComponent<Animator>().SetBool("sprouted", false);

        lastAttack = Time.time;
        attacking = false;
    }

    IEnumerator summon()
    {
        GameObject spik = seedPool.getObj();
        if (spik != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.summon);
            spik.SetActive(true);
            spik.transform.position = physicalShootHole.transform.position;

            Vector3 dir = ((plr.transform.position) - physicalShootHole.transform.position).normalized * 15f;
            Vector2 lookAt = (plr.transform.position - physicalShootHole.transform.position);
            float ang = Mathf.Atan2(lookAt.y, lookAt.x) * Mathf.Rad2Deg;
            spik.transform.rotation = Quaternion.Euler(0, 0, ang - 90);
            spik.GetComponent<Rigidbody2D>().linearVelocity = dir;
            yield return new WaitForSeconds(3f);
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

        foreach (Transform po in seedPool.transform)
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

