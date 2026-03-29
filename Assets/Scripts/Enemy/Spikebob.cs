using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikebob : MonoBehaviour, IEnemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject leg1;
    [SerializeField] private GameObject leg2;
    private Rigidbody2D rb;
    private float dir = 0;

    private ProjPool spikePool;
    private ProjPool coinPool;

    private float lastAttack = 0;
    private bool attacking = false;

    [SerializeField] private GameObject coinImage;

    [SerializeField] private int worth;
    public Vector3 boundCenter { get; set; }
    public Vector3 bounds { get; set; }

    public int MaxHealth { get; set; }
    public bool dead = false;
    public int health { get; set; }

    public ParticleSystem hurtPars { get; set; }

    private GameObject plr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spikePool = GameObject.Find("Pools").transform.Find("Spikes").GetComponent<ProjPool>();
        coinPool = GameObject.Find("Pools").transform.Find("Coin").GetComponent<ProjPool>();
        MaxHealth = 10;
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

        if (rb.linearVelocity.x > 0.1f)
        {
            dir = 1;
        } else
        {
            dir = -1;
        }

        if (rb.linearVelocity != Vector2.zero)
        {
            leg1.transform.rotation = Quaternion.Euler(0, 0, 0 + dir * (Mathf.Sin((Time.time * 8)) * 30));
            leg2.transform.rotation = Quaternion.Euler(0, 0, 0 + dir * (Mathf.Sin((Time.time * 8) + 3.14f) * 30));
        }

        if (Time.time - lastAttack > 2 && !attacking && !dead && !plr.GetComponent<plrMovement>().dying)
        {
            attacking = true;
            int ran = Random.Range(1, 3);
            if (ran == 1)
            {
                StartCoroutine(shakeNShoot());
            }
            else if (ran == 2)
            {
                StartCoroutine(barrage());


            }
        }
    }

    IEnumerator barrage()
    {
        for (int i = 0; i < 10; i++)
        {
            if (plr.GetComponent<plrMovement>().dying || dead) {
                break;
            }

            StartCoroutine(iconAndAttack());
            yield return new WaitForSeconds(0.5f);
        }
        lastAttack = Time.time;
        attacking = false;
    }

    IEnumerator shakeNShoot()
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < 5; i++)
        {
            if (plr.GetComponent<plrMovement>().dying || dead)
            {
                break;
            }
            transform.position = pos + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);
            yield return new WaitForSeconds(0.1f);
        }
        transform.position = pos;
        GetComponent<OuchBox>().active = true;
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = boundCenter + new Vector3(Random.Range((-bounds.x + 1f) / 2, (bounds.x - 1f) / 2), Random.Range((-bounds.y + 1f) / 2, (bounds.y - 1f) / 2), Random.Range(-bounds.z / 2, bounds.z / 2));
            float start = Time.time;
            while ((transform.position - randomPoint).magnitude > 0.1 && (Time.time - start) < 1.5)
            {
                rb.linearVelocity = (randomPoint - transform.position).normalized * ((-0.2f * (i^2)) + (2 * i) + 5) * 0.5f;
                yield return new WaitForEndOfFrame();
            }
        }
        GetComponent<OuchBox>().active = false;

        rb.linearVelocity = Vector3.zero;
        attacking = false;
    }
    IEnumerator iconAndAttack()
    {
        Vector3 location = boundCenter + new Vector3(Random.Range(-bounds.x / 2, bounds.x / 2), Random.Range(-bounds.y / 2, bounds.y / 2), Random.Range(-bounds.z / 2, bounds.z / 2));
        GameObject spik = spikePool.getObj();
        if (spik != null)
        {
            spik.SetActive(true);
            spik.transform.position = location;
            spik.GetComponent<Spikes>().icon = true;
            yield return StartCoroutine(spik.GetComponent<Spikes>().cycle(0.4f));
            yield return new WaitForSeconds(0.2f);
            GameObject spik2 = spikePool.getObj();
            if (spik2 != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.summon);

                spik2.GetComponent<Spikes>().icon = false;
                spik2.SetActive(true);
                spik2.transform.position = location;
                yield return StartCoroutine(spik2.GetComponent<Spikes>().cycle(0.5f));

            }
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
        
        foreach (Transform po in spikePool.transform)
        {
            
                if (po.gameObject.activeSelf)
                {
                    po.gameObject.SetActive(false);
                }
            
        }
        IHan.instance.titleCard = true;
        print("dead");
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
        IHan.instance.titleCard = false;

        yield return GameObject.Find("BattleScreen").GetComponent<BattleBox>().toggleBox(false, gameObject);
        GameObject.Find("BattleScreen").SetActive(false);
        Destroy(gameObject);
    }
}
